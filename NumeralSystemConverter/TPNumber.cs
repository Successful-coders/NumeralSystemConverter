using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;

namespace NumeralSystemConverter
{
    class TPNumber
    {
        private const int MIN_RADIX = 2;
        private const int MAX_RADIX = 16;

        public static string zero = "0";

        private string value;
        private int radix;
        private int errorLength;


        public TPNumber() { }
        public TPNumber(double value, int radix, int errorLength)
        {
            if (radix < MIN_RADIX || radix > MAX_RADIX)
                return;

            this.radix = radix;
            this.value = ConverterFrom10.Convert(value, radix, errorLength);
            this.errorLength = errorLength;
        }
        public TPNumber(string value, string radix, string errorLength) : this(int.Parse(value), int.Parse(radix), int.Parse(errorLength)) { }


        public TPNumber Copy()
        {
            return new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
        }
        public TPNumber Add(TPNumber otherNumber)
        {
            TPNumber result = new TPNumber();

            double sum = ConverterTo10.Convert(Convert.ToString(otherNumber.value), otherNumber.radix) +
                ConverterTo10.Convert(Convert.ToString(value), radix);
            result.value = Convert.ToString(ConverterFrom10.Convert(sum, otherNumber.radix, otherNumber.errorLength));
            result.radix = otherNumber.radix;
            result.errorLength = Math.Max(otherNumber.errorLength, errorLength);

            return result;
        }
        public TPNumber Multiply(TPNumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(otherNumber.value), otherNumber.radix) *
                ConverterTo10.Convert(Convert.ToString(value), radix), otherNumber.radix, otherNumber.errorLength));
            result.radix = otherNumber.radix;
            result.errorLength = otherNumber.errorLength + errorLength;

            return result;
        }
        public TPNumber Subtract(TPNumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(value), radix) -
                ConverterTo10.Convert(Convert.ToString(otherNumber.value), otherNumber.radix), otherNumber.radix, otherNumber.errorLength));
            result.radix = otherNumber.radix;
            result.errorLength = Math.Max(otherNumber.errorLength, errorLength);

            return result;
        }
        public TPNumber Divide(TPNumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(value), radix) /
                ConverterTo10.Convert(Convert.ToString(otherNumber.value), otherNumber.radix), otherNumber.radix, otherNumber.errorLength));
            result.radix = otherNumber.radix;
            result.errorLength = Math.Max(otherNumber.errorLength, errorLength);

            return result;
        }
        public TPNumber Inverse()
        {
            errorLength = 15;

            TPNumber result = new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
            result.value = Convert.ToString(ConverterFrom10.Convert(Math.Round(1 / ConverterTo10.Convert(value, radix), errorLength), radix, errorLength));

            return result;
        }
        public TPNumber Square()
        {
            errorLength = 15;

            TPNumber result = new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
            result.value = Convert.ToString(ConverterFrom10.Convert(Math.Round(Math.Pow(ConverterTo10.Convert(value, radix), 0.5), errorLength), radix, errorLength));

            return result;
        }


        public double ValueNumber
        {
            get
            {
                return ConverterTo10.Convert(value, radix);
            }
        }
        public string ValueString
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public int RadixNumber
        {
            get
            {
                return radix;
            }
            set
            {
                if (radix >= MIN_RADIX && radix <= MAX_RADIX)
                {
                    radix = value;
                }
            }
        }
        public string RadixString
        {
            get
            {
                return radix.ToString();
            }
            set
            {
                int intRadix = int.Parse(value);
                if (intRadix >= MIN_RADIX && intRadix <= MAX_RADIX)
                {
                    radix = intRadix;
                }
            }
        }

        public int ErrorLengthNumber
        {
            get
            {
                return errorLength;
            }
            set
            {
                if (value >= 0)
                {
                    errorLength = value;
                }
            }
        }
        public string ErrorLengthString
        {
            get
            {
                return errorLength.ToString();
            }
            set
            {
                int errorLengthInt = int.Parse(value);
                if (errorLengthInt >= 0)
                {
                    errorLength = errorLengthInt;
                }
            }
        }
    }
}
