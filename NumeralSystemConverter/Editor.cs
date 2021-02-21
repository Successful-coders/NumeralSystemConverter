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
        //Разделитель целой и дробной частей.
        private const char POINT_CHAR = '.';
        //Ноль.
        private const string ZERO = "0";

        //Поле для хранения редактируемого числа.
        private string number = "";
        //Точность представления результата.
        private int error = 0;


        /// <summary>
        /// Добавить цифру.
        /// </summary>
        public string AddDigit(int number, int radix)
        {
            this.number += ConverterFrom10.Convert(number, radix);
            return this.number;
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
            if (number.Length > 0)
                number = number.Remove(number.Length - 1);
            return number;
        }
        /// <summary>
        /// Очистить редактируемое число.
        /// </summary>
        public string Clear()
        {
            number = "";
            return number;
        }

        //Выполнить команду редактирования.
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
                        AddDigit(commandIndex, 16);
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
                    if (number != string.Empty)
                        RemoveLastSymbol();
                    if (number.Contains(POINT_CHAR))
                        error--;
                    break;
                case 18:
                    Clear();
                    error = 0;
                    break;
                case 20:
                    if (number.Length > 0 && (number != ZERO))
                    {
                        if (number[0] != '-')
                            number = "-" + number;
                        else
                        {
                            number = number.Split('-')[1];
                        }
                    }
                    break;
                default:
                    break;
            }
            return number;
        }


        public string Number
        {
            get
            {
                return number;
            }
        }
    }
}
