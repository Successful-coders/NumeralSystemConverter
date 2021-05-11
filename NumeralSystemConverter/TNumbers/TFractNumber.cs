using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter.TNumbers
{
    class TFractNumber : TANumber
    {
        private TPNumber numerator;
        private TPNumber denominator;


        public TFractNumber() : this(new TPNumber(0), new TPNumber(1))
        {

        }
        public TFractNumber(TPNumber numerator, TPNumber denominator)
        {
            this.numerator = (TPNumber)numerator.Copy();
            this.denominator = (TPNumber)denominator.Copy();

            int gcd = CalculateGCD(Convert.ToInt32(numerator.ValueNumber), Convert.ToInt32(denominator.ValueNumber));

            this.numerator = (TPNumber)numerator.Divide(new TPNumber(gcd, 10, 0));
            this.denominator = (TPNumber)denominator.Divide(new TPNumber(gcd, 10, 0));
        }


        public override TANumber Copy()
        {
            return new TFractNumber((TPNumber)numerator.Copy(), (TPNumber)denominator.Copy());
        }
        public override TANumber Add(TANumber otherNumber)
        {
            TPNumber numerator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber 
                + this.denominator.ValueNumber * (otherNumber as TFractNumber).numerator.ValueNumber, this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.denominator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
                this.denominator.RadixNumber, this.denominator.ErrorLengthNumber);

            return new TFractNumber(numerator, denominator);
        }
        public override TANumber Multiply(TANumber otherNumber)
        {
            TPNumber numerator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).numerator.ValueNumber,
                this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.denominator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
                this.denominator.RadixNumber, this.denominator.ErrorLengthNumber);

            return new TFractNumber(numerator, denominator);
        }
        public override TANumber Subtract(TANumber otherNumber)
        {
            TPNumber numerator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber
                - this.denominator.ValueNumber * (otherNumber as TFractNumber).numerator.ValueNumber, this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.denominator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
                this.denominator.RadixNumber, this.denominator.ErrorLengthNumber);

            return new TFractNumber(numerator, denominator);
        }
        public override TANumber Divide(TANumber otherNumber)
        {
            TPNumber numerator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
                this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.denominator.ValueNumber * (otherNumber as TFractNumber).numerator.ValueNumber,
                this.denominator.RadixNumber, this.denominator.ErrorLengthNumber);

            return new TFractNumber(numerator, denominator);
        }
        public override TANumber Inverse()
        {
            TPNumber numerator = (TPNumber)this.denominator.Copy();
            TPNumber denominator = (TPNumber)this.numerator.Copy();

            return new TFractNumber(numerator, denominator);
        }
        public override TANumber Square()
        {
            TPNumber numerator = new TPNumber(this.numerator.Square().ValueNumber,
                this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.denominator.Square().ValueNumber,
                this.denominator.RadixNumber, this.denominator.ErrorLengthNumber);

            return new TFractNumber(numerator, denominator);
        }
        public override bool Equals(TANumber other)
        {
            return (other as TFractNumber).numerator.Equals(this.numerator) && (other as TFractNumber).denominator.Equals(this.denominator);
        }
        public override string ToString()
        {
            return numerator + "/" + denominator;
        }

        private int CalculateGCD(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            for (;;)
            {
                int remainder = a % b;
                if (remainder == 0) return b;
                a = b;
                b = remainder;
            };
        }


        public override double ValueNumber => numerator.ValueNumber / denominator.ValueNumber;
        public override string ValueString
        { 
            get
            {
                if (!numerator.IsZero && denominator.IsZero)
                {
                    return "Деление на ноль!";
                }
                else
                {
                    return this.ToString();
                }
            }
            set
            {
                string[] stringValues = value.Split('/');
                numerator = new TPNumber(int.Parse(stringValues[0]));
                if (stringValues.Length >= 2 && !string.IsNullOrEmpty(stringValues[1]))
                {
                    denominator = new TPNumber(int.Parse(stringValues[1]));
                }
                else
                {
                    denominator = new TPNumber(1);
                }
            }
        }
        public override int RadixNumber { get => 10; set => this.ToString(); }
        public override string RadixString { get => "10"; set => this.ToString(); }
        public override int ErrorLengthNumber { get => 0; set => this.ToString(); }
        public override string ErrorLengthString { get => "0"; set => this.ToString(); }
        public override bool IsZero => numerator.IsZero;
    }
}
