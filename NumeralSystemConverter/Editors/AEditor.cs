using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NumeralSystemConverter.Editors.Constants;

namespace NumeralSystemConverter.Editors
{
    public abstract class AEditor
    {
        public State state;

        //Поле для хранения редактируемого числа.
        protected string number = "0";
        //Точность представления результата.
        protected int error = 0;


        /// <summary>
        /// Добавить цифру.
        /// </summary>
        public abstract string AddDigit(int number, int radix);
        /// <summary>
        /// Добавить символ.
        /// </summary>
        public abstract string AddSymbol(int symbolNumber);
        /// <summary>
        /// Добавить ноль.
        /// </summary>
        public virtual string AddZero()
        {
            number += ZERO;
            return number;
        }
        /// <summary>
        /// Добавить разделитель.
        /// </summary>
        public virtual string AddPoint()
        {
            number += POINT_CHAR;
            return number;
        }
        /// <summary>
        /// Удалить символ справа
        /// </summary>
        public virtual string RemoveLastSymbol()
        {
            if (number.Length > 0 && number != "0")
                number = number.Remove(number.Length - 1);
            return number;
        }
        /// <summary>
        /// Очистить редактируемое число.
        /// </summary>
        public virtual string Clear()
        {
            number = ZERO;
            return number;
        }
        /// <summary>
        /// Изменить знак.
        /// </summary>
        public virtual string ChangeSign()
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
        public abstract string Edit(int commandIndex);

        protected string GetNumberSymbol(int number)
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
