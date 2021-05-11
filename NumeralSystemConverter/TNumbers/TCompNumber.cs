using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter.TNumbers
{
    class TCompNumber : TANumber
    {
        private TPNumber realPart;
        private TPNumber imagePart;


        public TCompNumber() : base()
        {

        }
        public TCompNumber(TPNumber realPart, TPNumber imagePart)
        {
            this.realPart = (TPNumber)realPart.Copy();
            this.imagePart = (TPNumber)imagePart.Copy();
        }


        public override TANumber Copy()
        {
            return new TCompNumber((TPNumber)realPart.Copy(), (TPNumber)imagePart.Copy());
        }
        public override TANumber Add(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber(this.realPart.ValueNumber + (otherNumber as TCompNumber).realPart.ValueNumber,
                this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber(this.imagePart.ValueNumber + (otherNumber as TCompNumber).imagePart.ValueNumber,
                this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TFractNumber(realPart, imagePart);
        }
        public override TANumber Multiply(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber(this.realPart.ValueNumber * (otherNumber as TCompNumber).realPart.ValueNumber -
                this.imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber, this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber(this.realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber +
                this.imagePart.ValueNumber + (otherNumber as TCompNumber).realPart.ValueNumber, this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TFractNumber(realPart, imagePart);
        }
        public override TANumber Subtract(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber(this.realPart.ValueNumber - (otherNumber as TCompNumber).realPart.ValueNumber,
                this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber(this.imagePart.ValueNumber - (otherNumber as TCompNumber).imagePart.ValueNumber,
                this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TFractNumber(realPart, imagePart);
        }
        public override TANumber Divide(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber((this.realPart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber) /
                (this.imagePart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber),
                this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber((this.imagePart.ValueNumber * (otherNumber as TCompNumber).realPart.ValueNumber - this.realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber) /
                (this.imagePart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber),
                this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TFractNumber(realPart, imagePart);
        }
        public override TANumber Inverse()
        {
            var one = new TCompNumber(new TPNumber(1), new TPNumber(1));
            return one.Divide(this);
        }
        public override TANumber Square()
        {
            return this.Multiply(this);
        }
        public override bool Equals(TANumber other)
        {
            return (other as TCompNumber).realPart.Equals(this.realPart) && (other as TCompNumber).imagePart.Equals(this.imagePart);
        }
        public override string ToString()
        {
            return realPart + "+ i*" + imagePart;
        }


        public override double ValueNumber => throw new NotImplementedException();
        public override string ValueString { get => this.ToString(); set => throw new NotImplementedException(); }
        public override int RadixNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string RadixString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int ErrorLengthNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ErrorLengthString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsZero => realPart.IsZero && imagePart.IsZero;
    }
}
