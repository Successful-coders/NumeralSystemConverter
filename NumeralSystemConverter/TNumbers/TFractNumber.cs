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


        public TFractNumber() : base ()
        {

        }
        public TFractNumber(TPNumber numerator, TPNumber denominator)
        {
            this.numerator = (TPNumber)numerator.Copy();
            this.denominator = (TPNumber)denominator.Copy();
        }


        public override TANumber Copy()
        {
            return new TFractNumber((TPNumber)numerator.Copy(), (TPNumber)denominator.Copy());
        }
        public override TANumber Add(TANumber otherNumber)
        {
            TPNumber numerator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber 
                + this.denominator.ValueNumber * (otherNumber as TFractNumber).numerator.ValueNumber, this.numerator.RadixNumber, this.numerator.ErrorLengthNumber);
            TPNumber denominator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
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
            TPNumber denominator = new TPNumber(this.numerator.ValueNumber * (otherNumber as TFractNumber).denominator.ValueNumber,
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


        public override double ValueNumber => throw new NotImplementedException();
        public override string ValueString { get => this.ToString(); set => throw new NotImplementedException(); }
        public override int RadixNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string RadixString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int ErrorLengthNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ErrorLengthString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsZero => numerator.IsZero;
    }
}
