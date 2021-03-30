using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class TProcessor<T>
        where T : TPNumber
    {
        private T leftOperand;
        private T rightOperand;
        private OperationState state;


        public TProcessor(int radix)
        {
            leftOperand = new TPNumber(0, radix, 0) as T;
            rightOperand = new TPNumber(0, radix, 0) as T;
        }


        public void ResetProcessor()
        {
            leftOperand = new TPNumber(0, 10, 0) as T;
            rightOperand = new TPNumber(0, 10, 0) as T;

            ResetOperation();
        }
        public void ResetOperation()
        {
            state = OperationState.None;
        }
        public void Operate()
        {
            if (state == OperationState.None)
                return;

            if (state == OperationState.Add)
            {
                leftOperand = (T)leftOperand.Add(rightOperand);
            }
            else if (state == OperationState.Subtract)
            {
                leftOperand = (T)leftOperand.Subtract(rightOperand);
            }
            else if (state == OperationState.Multiply)
            {
                leftOperand = (T)leftOperand.Multiply(rightOperand);
            }
            else if (state == OperationState.Divide)
            {
                leftOperand = (T)leftOperand.Divide(rightOperand);
            }
        }
        public void CalculateFunction(/*bool isRignt*/)
        {
            if (state == OperationState.None)
                return;

            //if (!isRignt)
            //{
                if (state == OperationState.Inverse)
                {
                    leftOperand = (T)leftOperand.Inverse();
                }
                if (state == OperationState.Square)
                {
                    leftOperand = (T)leftOperand.Square();
                }
            //}
            //else
            //{
            //    if (state == OperationState.Inverse)
            //    {
            //        leftOperand = (T)leftOperand.Inverse();
            //    }
            //    if (state == OperationState.Square)
            //    {
            //        leftOperand = (T)leftOperand.Square();
            //    }
            //}
        }


        public T LeftOperand { get => leftOperand; set => leftOperand = (T)new TPNumber(value.ValueNumber, value.RadixNumber, value.ErrorLengthNumber); }
        public T RightOperand { get => rightOperand; set => rightOperand = (T)new TPNumber(value.ValueNumber, value.RadixNumber, value.ErrorLengthNumber); }
        public OperationState State { get => state; set => state = value; }



        public enum OperationState
        {
            None,
            Add,
            Subtract,
            Multiply,
            Divide,
            Inverse,
            Square,
        }
    }
}
