﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NumeralSystemConverter.Converter;

namespace NumeralSystemConverter
{
    class Control
    {
        private const int DEFAULT_RADIX = 10;
        private const int DEFAULT_ERROR = 10;

        private Editor editor = new Editor();
        public TProcessor<TPNumber> processor = new TProcessor<TPNumber>(DEFAULT_RADIX);
        private Memory<TPNumber> memory = new Memory<TPNumber>();
        private TPNumber number = new TPNumber(0, 10, 0);
        private History history = new History();


        public Control()
        {
            State = StateType.Starting;
            Radix = DEFAULT_RADIX;
        }


        public string DoCommand(int commandIndex, ref string clipboard, ref Memory<TPNumber>.FState memoryState)
        {
            string str;

            switch (commandIndex)
            {
                case int n when (n >= 0 && n <= 20):
                    str =  DoEditorCommand(commandIndex);
                    break;
                case int n when (n >= 21 && n <= 24):
                    if (processor.State != (TProcessor<TPNumber>.OperationState)commandIndex - 20 && editor.state == Editor.State.EditRight)
                        str = DoExpresion(commandIndex - 20);
                    else if (processor.State != (TProcessor<TPNumber>.OperationState)commandIndex - 20 && editor.state == Editor.State.Choose)
                        str = DoExpresion((int)processor.State);
                    str = Prepare(editor.Number, commandIndex - 20);
                    break;
                case int n when (n >= 25 && n <= 26):
                    str = DoFunction(commandIndex - 20);
                    break;
                case int n when (n >= 27 && n <= 27):
                    str = DoExpresion((int)processor.State);
                    break;
                case int n when (n >= 28 && n <= 28):
                    str = Reset();
                    break;
                case int n when (n >= 29 && n <= 33):
                    str = DoMemoryCommand(commandIndex - 29, ref memoryState);
                    break;
                case int n when (n >= 34 && n <= 36):
                    str = DoClipboardCommand(commandIndex -34, ref clipboard);
                    break;
                default:
                    str = number.ValueString;//TODO
                    break;
            }

            return str;
        }
        public string DoEditorCommand(int commandIndex)
        {
            State = StateType.Editing;

            return editor.Edit(commandIndex);
        }
        public string DoFunction(int commandIndex)
        {
            State = StateType.FunctionDone;

            processor.State = (TProcessor<TPNumber>.OperationState)commandIndex;

            string number = editor.Number;

            string res = "";
            string record = "";

            if (editor.state == Editor.State.EditRight)
            {
                processor.RightOperand.ValueString = number;
                if (commandIndex == 6)
                {
                    record = "Sqr( " + number + " ) = ";
                    processor.CalculateFunction();
                }
                else
                {
                    record += "1 / " + number + " = ";
                    processor.CalculateFunction();
                }
                res += processor.RightOperand.ValueNumber;
            }
            else
            {
                if (editor.state != Editor.State.Print)
                    processor.LeftOperand.ValueString = number;

                if (commandIndex == 6)
                {
                    record = "Sqr( " + number + " ) = ";
                    processor.CalculateFunction();
                }
                else
                {
                    record += "1 / " + number + " = ";
                    processor.CalculateFunction();
                }
                res += processor.LeftOperand.ValueNumber;
            }

            editor.Number = res;

            if (editor.state != Editor.State.EditRight)
                editor.state = Editor.State.Print;

            record += res;
            history.AddRecord(new Record(record));

            return res;
        }
        public string DoExpresion(int commandIndex)
        {
            State = StateType.ExpressionDone;

            processor.State = (TProcessor<TPNumber>.OperationState)commandIndex;

            string number = editor.Number;
            string result = "";

            if (processor.State != TProcessor<TPNumber>.OperationState.None)
            {
                if (processor.State == TProcessor<TPNumber>.OperationState.Divide && number == "0")
                {
                    editor.state = Editor.State.EditLeft;
                    return "Деление на ноль!";
                }

                string rec = processor.LeftOperand.ValueString;

                switch ((int)editor.state)
                {
                    case 2:
                        if (number != "")
                            processor.RightOperand.ValueString = number;
                        else
                            processor.RightOperand.ValueString = processor.LeftOperand.ValueString;

                        break;
                    case 1:
                        processor.RightOperand.ValueString = processor.LeftOperand.ValueString;
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
                editor.state = Editor.State.Print;
            }

            return result;
        }
        public string Reset()
        {
            State = StateType.Starting;

            return number.ValueString;
        }
        public string DoMemoryCommand(int commandIndex, ref Memory<TPNumber>.FState state)
        {
            State = StateType.ValueDone;//TODO

            memory.State = state;

            string str = editor.Number;
            TPNumber buf = new TPNumber(str, processor.LeftOperand.RadixString, "0");

            switch (commandIndex)
            {
                case 0:
                    {
                        memory.Clear();
                        editor.Clear();

                        return editor.Number;
                    }
                case 1:
                    {
                        return PopMemory();
                    }
                case 2:
                    {

                        memory.Store(buf);
                        break;
                    }
                case 3:
                    {
                        memory.Add(buf);
                        break;
                    }
            }

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
            if (editor.state != Editor.State.EditLeft)
                editor.state = Editor.State.EditRight;

            return res;
        }
        private string Prepare(string number, int command)
        {
            string result = "";
            string record = "";

            if (editor.state != Editor.State.Print)
            {
                if (editor.state == Editor.State.EditRight)
                {
                    processor.LeftOperand = processor.RightOperand.Copy();
                    processor.RightOperand = processor.LeftOperand.Copy();

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
                    editor.state = Editor.State.Choose;
                }
                else
                {
                    if (editor.state == Editor.State.Choose)
                    {
                        processor.RightOperand.ValueString = number;
                        result = processor.LeftOperand.ValueString;
                        editor.state = Editor.State.EditRight;
                    }
                    else
                    {
                        processor.LeftOperand.ValueString = number;
                        result = number;
                        editor.state = Editor.State.Choose;
                    }
                }
            }
            else
            {
                processor.RightOperand = processor.LeftOperand.Copy();
                result = processor.LeftOperand.ValueString;
                editor.state = Editor.State.EditRight;
            }

            editor.Clear();

            record += " = " + result;
            if (record.Length >= 9)
                history.AddRecord(new Record(record));

            editor.state = Editor.State.Choose;
            processor.State = (TProcessor<TPNumber>.OperationState)command;
            return result;
        }



        //Свойство для чтения и записи состояние Конвертера.
        public StateType State { get; set; }
        public int Radix { get; set; }
        public Editor Editor => editor;
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
