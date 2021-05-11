using NumeralSystemConverter.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NumeralSystemConverter.Editors.Constants;

namespace NumeralSystemConverter.Editors
{
    class PEditor : AEditor
    {
        public override string AddDigit(int number, int radix)
        {
            this.number += ConverterFrom10.Convert(number, radix);
            return this.number;
        }
        public override string AddSymbol(int symbolNumber)
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
        public override string AddZero()
        {
            number += ZERO;
            return number;
        }
        public override string AddPoint()
        {
            number += POINT_CHAR;
            return number;
        }
        public override string RemoveLastSymbol()
        {
            if (number.Length > 0 && number != "0")
                number = number.Remove(number.Length - 1);
            return number;
        }
        public override string Clear()
        {
            number = ZERO;
            return number;
        }
        public override string ChangeSign()
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
        public override string Edit(int commandIndex)
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
    }
}
