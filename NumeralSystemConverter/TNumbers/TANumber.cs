using NumeralSystemConverter.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NumeralSystemConverter.TNumbers.Constants;

namespace NumeralSystemConverter.TNumbers
{
    public abstract class TANumber : IEquatable<TANumber>
    {
        public abstract TANumber Copy();
        public abstract TANumber Add(TANumber otherNumber);
        public abstract TANumber Multiply(TANumber otherNumber);
        public abstract TANumber Subtract(TANumber otherNumber);
        public abstract TANumber Divide(TANumber otherNumber);
        public abstract TANumber Inverse();
        public abstract TANumber Square();
        public abstract bool Equals(TANumber other);


        public abstract double ValueNumber
        {
            get;
        }
        public abstract string ValueString
        {
            get;
            set;
        }
        public abstract int RadixNumber
        {
            get;
            set;
        }
        public abstract string RadixString
        {
            get;
            set;
        }
        public abstract int ErrorLengthNumber
        {
            get;
            set;
        }
        public abstract string ErrorLengthString
        {
            get;
            set;
        }
        public abstract bool IsZero { get; }
    }
}
