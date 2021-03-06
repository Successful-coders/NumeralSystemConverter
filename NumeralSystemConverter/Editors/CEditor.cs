﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NumeralSystemConverter.Editors.Constants;

namespace NumeralSystemConverter.Editors
{
    public class CEditor : AEditor
    {
        public override string AddDigit(int number, int radix = 10)
        {
            this.number += number.ToString();
            return this.number;
        }
        public override string AddSymbol(int symbolNumber)
        {
            number += symbolNumber.ToString();
            return number;
        }
        public override string Edit(int commandIndex)
        {
            switch (commandIndex)
            {
                case 0:
                    if (number != ZERO && !number.Contains(" + i*0"))
                        AddZero();
                    if (number.Contains(POINT_CHAR))
                        error++;
                    break;
                case int n when (n >= 1 && n <= 15):
                    if (number != ZERO && !number.Contains(" + i*0"))
                    {
                        AddSymbol(commandIndex);
                    }
                    else
                    {
                        number = number.Substring(0, number.Length - 1);
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
                case 28:
                    if (!number.Contains(" + i*"))
                    {
                        number += " + i*";
                    }
                    break;
                case 40:
                    if (!number.Contains(" + i*-") && number.Length > 5)
                    {
                        number += "-";
                    }
                    break;
                default:
                    break;
            }
            return number;
        }
        public override string RemoveLastSymbol()
        {
            if (number.Length > 0 && number != "0")
            {
                if (number.Contains(" + i*") && (number.IndexOf(" + i*") + 5) == number.Length)
                {
                    number = number.Remove(number.Length - 5);
                }
                else
                {
                    number = number.Remove(number.Length - 1);
                }
            }

            if (number.Length == 0)
            {
                number = "0";
            }
            return number;
        }
    }
}
