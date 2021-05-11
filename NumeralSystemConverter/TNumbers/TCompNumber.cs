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


        public TCompNumber() : this(new TPNumber(), new TPNumber())
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

            return new TCompNumber(realPart, imagePart);
        }
        public override TANumber Multiply(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber(this.realPart.ValueNumber * (otherNumber as TCompNumber).realPart.ValueNumber -
                this.imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber, this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber(this.realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber +
                this.imagePart.ValueNumber * (otherNumber as TCompNumber).realPart.ValueNumber, this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TCompNumber(realPart, imagePart);
        }
        public override TANumber Subtract(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber(this.realPart.ValueNumber - (otherNumber as TCompNumber).realPart.ValueNumber,
                this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber(this.imagePart.ValueNumber - (otherNumber as TCompNumber).imagePart.ValueNumber,
                this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TCompNumber(realPart, imagePart);
        }
        public override TANumber Divide(TANumber otherNumber)
        {
            TPNumber realPart = new TPNumber((this.realPart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber) /
                (this.imagePart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber),
                this.realPart.RadixNumber, this.realPart.ErrorLengthNumber);
            TPNumber imagePart = new TPNumber((this.imagePart.ValueNumber * (otherNumber as TCompNumber).realPart.ValueNumber - this.realPart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber) /
                (this.imagePart.ValueNumber * this.imagePart.ValueNumber + (otherNumber as TCompNumber).imagePart.ValueNumber * (otherNumber as TCompNumber).imagePart.ValueNumber),
                this.imagePart.RadixNumber, this.imagePart.ErrorLengthNumber);

            return new TCompNumber(realPart, imagePart);
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
            return realPart + " + i*" + imagePart;
        }


        public override double ValueNumber => realPart.ValueNumber;
        public override string ValueString
        {
            get
            {
                return this.ToString();
            }
            set
            {
                string[] stringValues = value.Split(new string[] { " + i*" }, StringSplitOptions.None);
                realPart = new TPNumber(int.Parse(stringValues[0]));
                if (stringValues.Length >= 2 && !string.IsNullOrEmpty(stringValues[1]))
                {
                    imagePart = new TPNumber(int.Parse(stringValues[1]));
                }
                else
                {
                    imagePart = new TPNumber(0);
                }
            }
        }
        public override int RadixNumber { get => 10; set => this.ToString(); }
        public override string RadixString { get => "10"; set => this.ToString(); }
        public override int ErrorLengthNumber { get => 0; set => this.ToString(); }
        public override string ErrorLengthString { get => "0"; set => this.ToString(); }
        public override bool IsZero => realPart.IsZero && imagePart.IsZero;
    }
}
