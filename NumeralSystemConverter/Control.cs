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

        private PEditor editor = new PEditor();
        public TProcessor<TPNumber> processor = new TProcessor<TPNumber>(DEFAULT_RADIX);
        private Memory<TPNumber> memory = new Memory<TPNumber>();
        private Memory<TPNumber>.FState memoryState = Memory<TPNumber>.FState.Off;
        private TPNumber number = new TPNumber(0, 10, 0);
        private History history = new History();
        private int prevCommand = -1;
        private int prevOperation;
        private int prevBinaryOperation;


        public Control()
        {
            State = StateType.Starting;
            Radix = DEFAULT_RADIX;
        }


        public string DoCommand(int commandIndex, ref string clipboard)
        {
            string str;
            SetRadix();

            switch (commandIndex)
            {
                case int n when (n >= 0 && n <= 20):
                    str =  DoEditorCommand(commandIndex);
                    break;
                case int n when (n >= 21 && n <= 24):
                    if (processor.State != (TProcessor<TPNumber>.OperationState)commandIndex - 20 && editor.state == AEditor.State.EditRight)
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
                case int n when (n >= 28 && n <= 28):
                    str = Reset();
                    break;
                case int n when (n >= 29 && n <= 33):
                    str = DoMemoryCommand(commandIndex - 29);
                    break;
                case int n when (n >= 34 && n <= 36):
                    str = DoClipboardCommand(commandIndex -34, ref clipboard);
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

            if (prevCommand == 27 || (prevCommand >= 21 && prevCommand <= 24))
                editor.Clear();

            return editor.Edit(commandIndex);
        }
        public string DoFunction(int commandIndex)
        {
            State = StateType.FunctionDone;

            processor.State = (TProcessor<TPNumber>.OperationState)commandIndex;

            string number = editor.Number;

            string res = "";
            string record = "";

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
                res += processor.RightOperand.ValueNumber;
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
                res += processor.LeftOperand.ValueNumber;
            }
            else
            {
                processor.RightOperand.ValueString = number;

                processor.State = (TProcessor<TPNumber>.OperationState)commandIndex;
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
                res += processor.RightOperand.ValueNumber;
            }

            editor.Number = res;

            if (editor.state != AEditor.State.EditRight && editor.state != AEditor.State.Choose)
                editor.state = AEditor.State.Print;

            record += res;
            history.AddRecord(new Record(record));

            prevOperation = (int)processor.State;

            return res;
        }
        public string DoExpresion(int commandIndex)
        {
            State = StateType.ExpressionDone;

            processor.State = (TProcessor<TPNumber>.OperationState)commandIndex;

            string number = editor.Number;
            string result = "";

            processor.RightOperand.ErrorLengthNumber = processor.LeftOperand.ErrorLengthNumber = Error;
            if (processor.State != TProcessor<TPNumber>.OperationState.None)
            {
                if (processor.State == TProcessor<TPNumber>.OperationState.Divide && number == "0")
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
                            processor.State = (TProcessor<TPNumber>.OperationState)prevBinaryOperation;
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
                        processor.State = (TProcessor<TPNumber>.OperationState)prevBinaryOperation;
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
            processor.State = TProcessor<TPNumber>.OperationState.None;

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
            State = StateType.ValueDone;//TODO

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

            if (processor.State == TProcessor<TPNumber>.OperationState.None)
            {
                processor.LeftOperand = memory.Number;
                res = processor.LeftOperand.ValueNumber.ToString();
                editor.Number = res;
            }
            else
            {
                processor.RightOperand = memory.Number;
                res = processor.RightOperand.ValueNumber.ToString();
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
                processor.RightOperand = (TPNumber)processor.LeftOperand.Copy();
                result = processor.LeftOperand.ValueString;
                editor.state = AEditor.State.EditRight;
            }

            editor.Clear();

            record += " = " + result;
            if (record.Length >= 9)
                history.AddRecord(new Record(record));

            editor.state = AEditor.State.Choose;
            processor.State = (TProcessor<TPNumber>.OperationState)command;
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
        public PEditor Editor => editor;
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
