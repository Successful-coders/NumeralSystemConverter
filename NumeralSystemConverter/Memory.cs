using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class Memory<T>
        where T : TPNumber
    {
        private T number;


        public Memory()
        {
            State = FState.Off;
            number = (T)new TPNumber(0, 10, 1);
        }


        public void Store(T number)
        {
            this.number = (T)number.Copy();

            State = FState.On;
        }
        public T Get()
        {
            T tmp;
            tmp = number;
            return tmp;
        }
        public void Add(T otherNumber)
        {
            if (State == FState.Off)
                number = (T)new TPNumber(otherNumber.ValueNumber, otherNumber.RadixNumber, otherNumber.ErrorLengthNumber);
            else
                number = (T)otherNumber.Add(number);

            State = FState.On;
        }
        public void Remove(T otherNumber)
        {
            if (State == FState.Off)
                number = (T)new TPNumber(-otherNumber.ValueNumber, otherNumber.RadixNumber, otherNumber.ErrorLengthNumber);
            else
                number = (T)otherNumber.Subtract(number);

            State = FState.On;
        }
        public void Clear()
        {
            State = FState.Off;
        }


        public T Number => number;
        public FState State { set; get; }


        public enum FState
        {
            Off,
            On,
        };
    }

}
