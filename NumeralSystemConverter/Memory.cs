using NumeralSystemConverter.TNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class Memory
    {
        private TANumber number;


        public Memory()
        {
            State = FState.Off;
            number = new TPNumber(0, 10, 1);
        }


        public void Store(TANumber number)
        {
            this.number = number.Copy();

            State = FState.On;
        }
        public TANumber Get()
        {
            return number.Copy();
        }
        public void Add(TANumber otherNumber)
        {
            if (State == FState.Off)
                number = otherNumber.Copy();
            else
                number = otherNumber.Add(number);

            State = FState.On;
        }
        public void Remove(TANumber otherNumber)
        {
            if (State == FState.Off)
                number = otherNumber.Copy();
            else
                number = otherNumber.Subtract(number);

            State = FState.On;
        }
        public void Clear()
        {
            State = FState.Off;
            number = new TPNumber(0, 10, 1);
        }


        public TANumber Number => number;
        public FState State { set; get; }


        public enum FState
        {
            Off,
            On,
        };
    }

}
