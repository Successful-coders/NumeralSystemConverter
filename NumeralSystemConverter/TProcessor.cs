using NumeralSystemConverter.TNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class TProcessor
    {
        private TANumber leftOperand;
        private TANumber rightOperand;
        private OperationState state;


        public void ResetProcessor()
        {
            leftOperand = new TPNumber(0, 10, 0);
            rightOperand = new TPNumber(0, 10, 0);

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
                leftOperand = leftOperand.Add(rightOperand);
            }
            else if (state == OperationState.Subtract)
            {
                leftOperand = leftOperand.Subtract(rightOperand);
            }
            else if (state == OperationState.Multiply)
            {
                leftOperand = leftOperand.Multiply(rightOperand);
            }
            else if (state == OperationState.Divide)
            {
                leftOperand = leftOperand.Divide(rightOperand);
            }
        }
        public void CalculateFunction(bool isRignt)
        {
            if (state == OperationState.None)
                return;

            if (!isRignt)
            {
                if (state == OperationState.Inverse)
                {
                    leftOperand = leftOperand.Inverse();
                }
                if (state == OperationState.Square)
                {
                    leftOperand = leftOperand.Square();
                }
            }
            else
            {
                if (state == OperationState.Inverse)
                {
                    rightOperand = rightOperand.Inverse();
                }
                if (state == OperationState.Square)
                {
                    rightOperand = rightOperand.Square();
                }
            }
        }


        public TANumber LeftOperand { get => leftOperand; set => leftOperand = value.Copy(); }
        public TANumber RightOperand { get => rightOperand; set => rightOperand = value.Copy(); }
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
