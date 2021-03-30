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
        //Преобразовать десятичное целое в с.сч. с основанием radix.
        public static string Convert(int number, int radix)
        {
            string ans = "";
            if (number == 0)
                ans = "0";
            else
                while (number != 0)
                {
                    ans = ConvertDigit(number % radix) + ans;
                    number /= radix;
                }
            return ans;
        }

        //Преобразовать десятичную дробь в с.сч. с основанием radix.
        public static string Convert1(double number, int radix, int roundLength)
        {
            string ans = "";
            while (roundLength != 0 && number != 0.0)
            {
                number *= radix;
                if (number >= 1)
                {
                    ans += ConvertDigit((int)number);
                    number -= (int)number;
                }
                else
                    ans += "0";
                roundLength--;
            }

            return ans;
        }

        //Преобразовать десятичное 
        //действительное число в с.сч. с осн. radix.
        public static string Convert(double number, int radix, int roundLength)
        {
            CheckRadixCorrect(radix);

            //if (number != 0)
            //    roundLength = number.ToString().Length - number.ToString().IndexOf('.') - 1;

            if (number == 0)
            {
                return "0";
            }
            else
            {
                string ans = "";
                if (number < 0)
                {
                    ans += "-";
                    number = -number;
                }
                string start = Convert((int)number, radix);
                string flt = Convert1(number - (int)number, radix, roundLength);
                ans += flt.Length > 0 ? start + "." + flt : start;
                
                if (ans.Contains('.'))
                {
                    return ans.TrimEnd(new char[] { '0' });
                }
                else
                {
                    return ans;
                }
            }
        }

        //Преобразовать целое в символ.
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
