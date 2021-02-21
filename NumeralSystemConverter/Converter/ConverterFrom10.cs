using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NumeralSystemConverter.Converter.Constants;

namespace NumeralSystemConverter.Converter
{
    public static class ConverterFrom10
    {
        /// <summary>
        /// Преобразовать действительное число в другую систему счисления
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="radix">Основание</param>
        /// <param name="roundLength">Точность преобразования дроби</param>
        /// <returns></returns>
        public static string Convert(double number, int radix, int roundLength)
        {
            CheckRadixCorrect(radix);

            if (roundLength < 0)
            {
                throw new Exception("Задана отрицательная точность");
            }
            if (number == 0)
            {
                return "0";
            }

            string sign = "";
            if (number < 0)
            {
                sign = "-";
                number *= -1;
            }

            StringBuilder convertedNumber = new StringBuilder();

            // Целая часть числа
            int wholePart = (int)Math.Floor(number);
            while (wholePart > 1)
            {
                convertedNumber.Insert(0, ConvertDigit(wholePart % radix));
                wholePart = wholePart / radix;
            }

            // Дробная часть числа
            double remainderPart = number % 1;
            if (remainderPart > Double.Epsilon || remainderPart < -Double.Epsilon)
            {
                convertedNumber.Append(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                double newDigit;
                for (int i = 0; i < roundLength; i++)
                {
                    newDigit = remainderPart * radix;
                    if (remainderPart < Double.Epsilon && remainderPart > -Double.Epsilon)
                        break;
                    convertedNumber.Append(ConvertDigit((int)Math.Floor(newDigit)));
                    remainderPart = newDigit % 1;
                }
            }

            return sign + convertedNumber.ToString();
        }
        /// <summary>
        /// Преобразовать целое число в другую систему счисления
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="radix">Основание</param>
        /// <returns></returns>
        public static string Convert(int number, int radix)
        {
            CheckRadixCorrect(radix);

            string sign = "";
            if(number < 0)
            {
                sign = "-";
                number *= -1;
            }
            string convertedNumber = "";

            do
            {
                convertedNumber = ConvertDigit(number % radix) + convertedNumber;
                number /= radix;
            }
            while (number > 0);

            return sign + convertedNumber;
        }

        private static char ConvertDigit(int digit)
        {
            return baseSymbols[digit];
        }
        private static void CheckRadixCorrect(int radix)
        {
            if (radix < MIN_RADIX || radix > MAX_RADIX)
            {
                throw new Exception($"Основание не принадлежит диапазону [{MIN_RADIX} ; {MAX_RADIX}]");
            }
        }
    }
}
