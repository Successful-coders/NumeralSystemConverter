using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NumeralSystemConverter.Converter;
using NumeralSystemConverter.Editors;
using NumeralSystemConverter.TNumbers;

namespace NumeralSystemConverter
{
    class Control
    {
        private const int DEFAULT_RADIX = 10;
        private const int DEFAULT_ERROR = 10;

        private AEditor editor = new PEditor();
        public TProcessor processor = new TProcessor();
        private Memory memory = new Memory();
        private Memory.FState memoryState = Memory.FState.Off;
        private TPNumber number = new TPNumber(0, DEFAULT_RADIX, 0);
        private History history = new History();
        private int prevCommand = -1;
        private int prevOperation;
        private int prevBinaryOperation;
        private string clipboard;


        public Control(AEditor editor)
        {
            this.editor = editor;

            State = StateType.Starting;
            Radix = DEFAULT_RADIX;
        }


        public string DoCommand(int commandIndex)
        {
            string str;
            SetRadix();

            switch (commandIndex)
            {
                case int n when ((n >= 0 && n <= 20) || n == 28 || ((editor is CEditor) && n == 40)):
                    str =  DoEditorCommand(commandIndex);
                    break;
                case int n when (n >= 21 && n <= 24):
                    if (processor.State != (TProcessor.OperationState)commandIndex - 20 && editor.state == AEditor.State.EditRight)
                        str = DoExpresion(commandIndex - 20);
                    else if (editor.state == AEditor.State.Choose)
                        str = DoExpresion((int)processor.State);
                    str = Prepare(editor.Number, commandIndex - 20);
                    break;
                case int n when (n >= 25 && n <= 26):
                    str = DoFunction(commandIndex - 20);
                    break;
                case int n when (n >= 27 && n <= 27):
                    if (prevCommand >= 21 && prevCommand <= 24)
                        editor.state = AEditor.State.EditRight;
                    str = DoExpresion(prevBinaryOperation);
                    editor.state = AEditor.State.EditLeft;
                    break;
                case int n when (n >= 29 && n <= 33):
                    str = DoMemoryCommand(commandIndex - 29);
                    break;
                case int n when (n >= 34 && n <= 36):
                    str = DoClipboardCommand(commandIndex - 34, ref clipboard);
                    break;
                default:
                    str = number.ValueString;
                    break;
            }

            if (prevCommand == 18 && commandIndex == 18)
                Reset();
            prevCommand = commandIndex;
            return str;
        }
        public string DoEditorCommand(int commandIndex)
        {
            State = StateType.Editing;

            if ((editor is CEditor) && commandIndex == 40)
            {
                return editor.Edit(commandIndex);
            }

            if (prevCommand == 27 || (prevCommand >= 21 && prevCommand <= 24))
                editor.Clear();

            return editor.Edit(commandIndex);
        }
        public string DoFunction(int commandIndex)
        {
            State = StateType.FunctionDone;

            processor.State = (TProcessor.OperationState)commandIndex;

            string number = editor.Number;

            string res = "";
            string record = "";

            if (number.Contains("/0"))
            {
                editor.state = AEditor.State.EditLeft;
                return "Деление на ноль!";
            }

            if (editor.state == AEditor.State.EditRight || editor.state == AEditor.State.Choose)
            {
                processor.RightOperand.ValueString = number;
                if (commandIndex == 6)
                {
                    record = "Sqr( " + number + " ) = ";
                    processor.CalculateFunction(true);
                }
                else
                {
                    record += "1 / " + number + " = ";
                    processor.CalculateFunction(true);
                }
                res = processor.RightOperand.ValueString;
            }
            else if (editor.state == AEditor.State.EditLeft)
            {
                if (editor.state != AEditor.State.Print)
                    processor.LeftOperand.ValueString = number;

                if (commandIndex == 6)
                {
                    record = "Sqr( " + number + " ) = ";
                    processor.CalculateFunction(false);
                }
                else
                {
                    record += "1 / " + number + " = ";
                    processor.CalculateFunction(false);
                }
                res = processor.LeftOperand.ValueString;
            }
            else
            {
                processor.RightOperand.ValueString = number;

                processor.State = (TProcessor.OperationState)commandIndex;
                if (commandIndex == 6)
                {
                    record = "Sqr( " + number + " ) = ";
                    processor.CalculateFunction(true);
                }
                else
                {
                    record += "1 / " + number + " = ";
                    processor.CalculateFunction(true);
                }
                res = processor.RightOperand.ValueString;
            }

            editor.Number = res;

            if (editor.state != AEditor.State.EditRight && editor.state != AEditor.State.EditLeft && editor.state != AEditor.State.Choose)
                editor.state = AEditor.State.Print;

            record += res;
            history.AddRecord(new Record(record));

            prevOperation = (int)processor.State;

            return res;
        }
        public string DoExpresion(int commandIndex)
        {
            State = StateType.ExpressionDone;

            processor.State = (TProcessor.OperationState)commandIndex;

            string number = editor.Number;
            string result = "";

            processor.RightOperand.ErrorLengthNumber = processor.LeftOperand.ErrorLengthNumber = Error;
            if (processor.State != TProcessor.OperationState.None)
            {
                if ((processor.State == TProcessor.OperationState.Divide && number == "0") || number.Contains("/0") || processor.LeftOperand.ValueString.Contains("/0"))
                {
                    editor.state = AEditor.State.EditLeft;
                    return "Деление на ноль!";
                }

                string rec = processor.LeftOperand.ValueString;

                switch (editor.state)
                {
                    case AEditor.State.Choose:
                        if (number != "")
                        {
                            processor.RightOperand.ValueString = number;
                            processor.State = (TProcessor.OperationState)prevBinaryOperation;
                        }
                        else
                            processor.RightOperand.ValueString = processor.LeftOperand.ValueString;

                        break;
                    case AEditor.State.EditRight:
                        break;
                    case 0:
                        break;
                    default:
                        processor.RightOperand.ValueString = number;
                        processor.State = (TProcessor.OperationState)prevBinaryOperation;
                        break;
                }

                processor.Operate();
                editor.Clear();
                result = processor.LeftOperand.ValueString;
                result = result.Replace(',', '.');


                switch ((int)processor.State)
                {
                    case 1: rec += " + "; break;
                    case 2: rec += " - "; break;
                    case 3: rec += " * "; break;
                    case 4: rec += " / "; break;
                }

                rec += processor.RightOperand.ValueString;
                rec += " = " + processor.LeftOperand.ValueString;
                history.AddRecord(new Record(rec));
                editor.Number = result;
                editor.state = AEditor.State.Print;
            }

            return result == "" ? editor.Number : result;
        }
        public string Reset()
        {
            State = StateType.Starting;
            number.RadixNumber = Radix;
            processor.State = TProcessor.OperationState.None;

            return number.ValueString;
        }
        public string DoMemoryCommand(int commandIndex)
        {
            State = StateType.ValueDone;//TODO

            memory.State = memoryState;

            string str = editor.Number;
            TPNumber buf = new TPNumber(str, processor.LeftOperand.RadixString, "0");

            switch (commandIndex)
            {
                case 0:
                    {
                        memory.Clear();

                        return editor.Number;
                    }
                case 1:
                    {
                        return PopMemory();
                    }
                case 2:
                    {
                        memory.Store(buf);
                        //editor.Clear();

                        break;
                    }
                case 3:
                    {
                        memory.Add(buf);
                        editor.Clear();

                        break;
                    }
                case 4:
                    {
                        memory.Remove(buf);
                        editor.Clear();

                        break;
                    }
            }

            memoryState = memory.State;

            return editor.Number;
        }
        public string DoClipboardCommand(int commandIndex, ref string clipboardValue)
        {
            State = StateType.ValueDone;//TODOTProcessor

            switch (commandIndex)
            {
                case 0:
                    {
                        Clipboard.SetText(clipboardValue);
                        break;
                    }
                case 1:
                    {
                        clipboardValue = Clipboard.GetText();
                        break;
                    }
                case 2:
                    {
                        Clipboard.Clear();
                        break;
                    }
            }

            return number.ValueString;
        }

        //Взять из памяти
        private string PopMemory()
        {
            string res;

            if (processor.State == TProcessor.OperationState.None)
            {
                processor.LeftOperand.ValueString = memory.Get().ValueString;
                res = processor.LeftOperand.ValueString;
                editor.Number = res;
            }
            else
            {
                processor.RightOperand.ValueString = memory.Get().ValueString;
                res = processor.RightOperand.ValueString;
                editor.Number = res;
            }
            if (editor.state != AEditor.State.EditLeft)
                editor.state = AEditor.State.EditRight;

            return res;
        }
        private string Prepare(string number, int command)
        {
            string result = "";
            string record = "";

            if (editor.state != AEditor.State.Print)
            {
                if (editor.state == AEditor.State.EditRight)
                {
                    processor.LeftOperand = (TPNumber)processor.RightOperand.Copy();
                    processor.RightOperand = (TPNumber)processor.LeftOperand.Copy();

                    record += processor.LeftOperand.ValueNumber;
                    switch ((int)processor.State)
                    {
                        case 1: record += " + "; break;
                        case 2: record += " - "; break;
                        case 3: record += " * "; break;
                        case 4: record += " / "; break;
                    }
                    record += processor.RightOperand.ValueString;
                    processor.Operate();
                    result = processor.LeftOperand.ValueString;
                    editor.state = AEditor.State.Choose;
                }
                else
                {
                    if (number.Contains("/0"))
                    {
                        editor.state = AEditor.State.EditLeft;
                        return "Деление на ноль!";
                    }

                    if (editor.state == AEditor.State.Choose)
                    {
                        processor.RightOperand.ValueString = number;
                        result = processor.LeftOperand.ValueString;
                        editor.state = AEditor.State.EditRight;
                    }
                    else
                    {
                        processor.LeftOperand.ValueString = number;
                        result = number;
                        editor.state = AEditor.State.Choose;
                    }
                }
            }
            else
            {
                processor.RightOperand = (TANumber)processor.LeftOperand.Copy();
                result = processor.LeftOperand.ValueString;
                editor.state = AEditor.State.EditRight;
            }

            editor.Clear();

            record += " = " + result;
            if (record.Length >= 9)
                history.AddRecord(new Record(record));

            editor.state = AEditor.State.Choose;
            processor.State = (TProcessor.OperationState)command;
            prevOperation = (int)processor.State;
            prevBinaryOperation = (int)processor.State;

            return result;
        }
        private void SetRadix()
        {
            if (editor.state == AEditor.State.Choose || editor.state == AEditor.State.EditRight || editor.state == AEditor.State.Print)
            {
                processor.RightOperand.RadixNumber = Radix;
            }
            else
            {
                processor.LeftOperand.RadixNumber = Radix;
            }
        }



        //Свойство для чтения и записи состояние Конвертера.
        public StateType State { get; set; }
        public int Radix { get; set; }
        public AEditor Editor => editor;
        public History History => history;

        //Точность представления результата.
        private int Error => editor.Error;


        public enum StateType
        {
            Starting,
            Editing,
            FunctionDone,
            ValueDone,
            ExpressionDone,
            OperationChanging,
            Error,
        }
    }
}
