using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;
using static NumeralSystemConverter.TNumbers.Constants;

namespace NumeralSystemConverter.TNumbers
{
    class TPNumber : TANumber
    {
        private const int DEFAULT_RADIX = 10;
        private const int DEFAULT_ERROR_LENGTH = 0;

        private string value;
        private int radix;
        private int errorLength;


        public TPNumber() : this (0)
        {

        }
        public TPNumber(double value) : this(value, DEFAULT_RADIX, DEFAULT_ERROR_LENGTH)
        {

        }
        public TPNumber(double value, int radix, int errorLength)
        {
            if (radix < MIN_RADIX || radix > MAX_RADIX)
                return;

            this.radix = radix;
            this.value = radix == DEFAULT_RADIX ? value.ToString() : ConverterFrom10.Convert(value, radix, errorLength);
            this.errorLength = errorLength;
        }
        public TPNumber(string value, string radix, string errorLength)
        {
            this.radix = int.Parse(radix);

            if (this.radix < MIN_RADIX || this.radix > MAX_RADIX)
                return;

            this.value = value;
            this.errorLength = int.Parse(errorLength);
        }


        public override TANumber Copy()
        {
            return new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
        }
        public override TANumber Add(TANumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.errorLength = otherNumber.ErrorLengthNumber + errorLength;
            double sum = ConverterTo10.Convert(Convert.ToString(otherNumber.ValueNumber), otherNumber.RadixNumber) +
                ConverterTo10.Convert(Convert.ToString(value), radix);
            result.value = Convert.ToString(ConverterFrom10.Convert(sum, otherNumber.RadixNumber, result.errorLength));
            result.radix = otherNumber.RadixNumber;
            result.errorLength = Math.Max(otherNumber.ErrorLengthNumber, errorLength);

            return result;
        }
        public override TANumber Multiply(TANumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.errorLength = otherNumber.ErrorLengthNumber + errorLength;
            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(otherNumber.ValueNumber), otherNumber.RadixNumber) *
                ConverterTo10.Convert(Convert.ToString(value), radix), otherNumber.RadixNumber, result.errorLength));
            result.radix = otherNumber.RadixNumber;

            return result;
        }
        public override TANumber Subtract(TANumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.errorLength = otherNumber.ErrorLengthNumber + errorLength;
            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(value), radix) -
                ConverterTo10.Convert(Convert.ToString(otherNumber.ValueNumber), otherNumber.RadixNumber), otherNumber.RadixNumber, result.errorLength));
            result.radix = otherNumber.RadixNumber;
            result.errorLength = Math.Max(otherNumber.ErrorLengthNumber, errorLength);

            return result;
        }
        public override TANumber Divide(TANumber otherNumber)
        {
            TPNumber result = new TPNumber();

            result.errorLength = otherNumber.ErrorLengthNumber + errorLength;
            result.value = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(value), radix) /
                ConverterTo10.Convert(Convert.ToString(otherNumber.ValueNumber), otherNumber.RadixNumber), otherNumber.RadixNumber, result.errorLength));
            result.radix = otherNumber.RadixNumber;

            return result;
        }
        public override TANumber Inverse()
        {
            errorLength = 15;

            TPNumber result = new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
            result.value = Convert.ToString(ConverterFrom10.Convert(Math.Round(1 / ConverterTo10.Convert(value, radix), errorLength), radix, errorLength));

            return result;
        }
        public override TANumber Square()
        {
            errorLength = 15;

            TPNumber result = new TPNumber(value, Convert.ToString(radix), Convert.ToString(errorLength));
            result.value = Convert.ToString(ConverterFrom10.Convert(Math.Round(Math.Pow(ConverterTo10.Convert(value, radix), 0.5), errorLength), radix, errorLength));

            return result;
        }
        public override bool Equals(TANumber other)
        {
            return value == (other as TPNumber).value && radix == (other as TPNumber).radix;
        }
        public override string ToString()
        {
            return value.ToString();
        }

        public override double ValueNumber
        {
            get
            {
                return ConverterTo10.Convert(value, radix);
            }
        }
        public override string ValueString
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
        public override int RadixNumber
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
        public override string RadixString
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
        public override int ErrorLengthNumber
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
        public override string ErrorLengthString
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
        public override bool IsZero => value == zero;
    }
}
