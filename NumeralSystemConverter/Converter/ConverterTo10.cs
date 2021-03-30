using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NumeralSystemConverter.Converter.Constants;

namespace NumeralSystemConverter.Converter
{
    public static class ConverterTo10
    {
        /// <summary>
        /// Преобразовать число в деситичную систему счисления
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="radix">Основание</param>
        /// <returns></returns>
        public static double Convert(string number, int radix)
        {
            if (number.Length == 0 || number == "0")
            {
                return 0;
            }
            else
            {
                double weight = 1;
                int sign = 1;
                int digitIndex = 0;
                string convertedNumber = "";
                char pointChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

                if (number[digitIndex] == '-')
                {
                    sign = -1;
                    digitIndex++;
                }
                if (number[digitIndex] != '0')
                {
                    for (; digitIndex < number.Length && number[digitIndex] != pointChar; digitIndex++)
                    {
                        convertedNumber += number[digitIndex];
                    }
                    weight = sign * Math.Pow(radix, sign > 0 ? digitIndex - 1 : digitIndex - 2);
                    if (digitIndex < number.Length)
                    {
                        if (number[digitIndex] == pointChar)
                        {
                            for (digitIndex += 1; digitIndex < number.Length; digitIndex++)
                            {
                                convertedNumber += number[digitIndex];
                            }
                        }
                    }
                }
                else
                {
                    bool isNotZero = false;
                    for (digitIndex += 2; digitIndex < number.Length; digitIndex++)
                    {
                        if (!isNotZero && number[digitIndex] != '0')
                        {
                            isNotZero = true;
                            weight = sign * Math.Pow(radix, 1 - digitIndex);
                        }
                        if (isNotZero)
                        {
                            convertedNumber += number[digitIndex];
                        }
                    }
                }

                double returnedNumber = Convert(convertedNumber, radix, weight);
                return returnedNumber;
            }
        }

        private static double Convert(string number, int radix, double weight)
        {
            if (number == "0")
            {
                return 0;
            }
            else
            {
                double result = 0;
                int i = 0;
                while (i < number.Length)
                {
                    result += ConvertDigit(number[i]) * weight;
                    weight /= radix;
                    i++;
                }

                return result;
            }
        }
        private static int ConvertDigit(char digit)
        {
            int decimalDigit = baseSymbols.ToList().FindIndex(x => x == digit);
            if (decimalDigit == -1)
            {
                throw new Exception("Задан некорректный символ");
            }
            else
            {
                return decimalDigit;
            }
        }
    }
}
