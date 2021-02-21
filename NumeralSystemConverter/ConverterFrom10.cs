using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class ConverterFrom10
    {
        private const int MIN_RADIX = 2;
        private const int MAX_RADIX = 16;


        private static char[] baseSymbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};



        /// <summary>
        /// Преобразовать действительное число в другую систему счисления
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="radix">Оснавание</param>
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

            StringBuilder convertedNumber = new StringBuilder();

            // Целая часть числа
            int wholePart = (int)Math.Floor(number);
            while (wholePart > 1)
            {
                convertedNumber.Insert(0, ConvertDigit(wholePart % radix, radix));
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
                    convertedNumber.Append(ConvertDigit((int)Math.Floor(newDigit), radix));
                    remainderPart = newDigit % 1;
                }
            }

            return convertedNumber.ToString();
        }
        /// <summary>
        /// Преобразовать целое число в другую систему счисления
        /// </summary>
        /// <param name="number">Число</param>
        /// <param name="radix">Оснавание</param>
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
                convertedNumber = ConvertDigit(number % radix, radix) + convertedNumber;
                number /= radix;
            }
            while (number > 0);

            return sign + convertedNumber;
        }
        /// <summary>
        /// Преобразовать цифру в другую систему счисления
        /// </summary>
        /// <param name="digit">Цифра</param>
        /// <param name="radix">Оснавание</param>
        /// <returns></returns>
        public static char ConvertDigit(int digit, int radix)
        {
            CheckRadixCorrect(radix);

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
