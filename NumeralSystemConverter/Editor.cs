using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumeralSystemConverter.Converter
{
    class Editor
    {
        private const int MAX_LENGTH = 16;
        //Разделитель целой и дробной частей.
        private const char POINT_CHAR = '.';
        //Ноль.
        private const string ZERO = "0";

        //Поле для хранения редактируемого числа.
        private string number = "0";
        //Точность представления результата.
        private int error = 0;
        public State state;


        /// <summary>
        /// Добавить цифру.
        /// </summary>
        public string AddDigit(int number, int radix)
        {
            this.number += ConverterFrom10.Convert(number, radix);
            return this.number;
        }
        public string AddSymbol(int symbolNumber)
        {
            if (number.Length < MAX_LENGTH)
            {
                if (number == "0")
                {
                    if (symbolNumber >= 10)
                        number = GetNumberSymbol(symbolNumber);
                    else if (symbolNumber != 0)
                        number += symbolNumber.ToString();
                }
                else
                {
                    if (symbolNumber >= 10)
                        number += GetNumberSymbol(symbolNumber);
                    else
                        number += symbolNumber.ToString();
                }
            }
            return number;

        }
        /// <summary>
        /// Добавить ноль.
        /// </summary>
        public string AddZero()
        {
            number += ZERO;
            return number;
        }
        /// <summary>
        /// Добавить разделитель.
        /// </summary>
        public string AddPoint()
        {
            number += POINT_CHAR;
            return number;
        }
        /// <summary>
        /// Удалить символ справа
        /// </summary>
        public string RemoveLastSymbol()
        {
            if (number.Length > 0 && number != "0")
                number = number.Remove(number.Length - 1);
            return number;
        }
        /// <summary>
        /// Очистить редактируемое число.
        /// </summary>
        public string Clear()
        {
            number = "0";
            return number;
        }
        /// <summary>
        /// Изменить знак.
        /// </summary>
        public string ChangeSign()
        {
            if (number.Length > 0 && number != ZERO)
            {
                if (number[0] == '-')
                {
                    number = number.Substring(1);
                }
                else
                {
                    number = '-' + number;
                }
            }

            return number;
        }
        /// <summary>
        /// Выполнить команду редактирования.
        /// </summary>
        public string Edit(int commandIndex)
        {
            switch (commandIndex)
            {
                case 0:
                    if (number != ZERO)
                        AddZero();
                    if (number.Contains(POINT_CHAR))
                        error++;
                    break;
                case int n when (n >= 1 && n <= 15):
                    if (number != ZERO)
                    {
                        AddSymbol(commandIndex);
                    }
                    else
                    {
                        number = "";
                        AddSymbol(commandIndex);
                    }

                    if (number.Contains(POINT_CHAR))
                        error++;
                    break;
                case 16:
                    if (number.Length > 0 && !number.Contains(POINT_CHAR))
                    {
                        AddPoint();
                        error = 0;
                    }
                    break;
                case 17:
                    ChangeSign();
                    break;
                case 18:
                    Clear();
                    error = 0;
                    break;
                case 19:
                    if (number != string.Empty)
                        RemoveLastSymbol();
                    if (number.Contains(POINT_CHAR))
                        error--;
                    break;
                case 20:
                    Clear();
                    error = 0;
                    break;
                default:
                    break;
            }
            return number;
        }

        private string GetNumberSymbol(int number)
        {
            string S = "";
            switch (number)
            {
                case 10: S = "A"; break;
                case 11: S = "B"; break;
                case 12: S = "C"; break;
                case 13: S = "D"; break;
                case 14: S = "E"; break;
                case 15: S = "F"; break;
                default: break;
            }
            return S;
        }



        public int Error => error;
        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
            }
        }
        public bool IsNumberZero => number == ZERO;


        public enum State
        {
            EditLeft,
            EditRight,
            Choose,
            Print,
        }
    }
}
