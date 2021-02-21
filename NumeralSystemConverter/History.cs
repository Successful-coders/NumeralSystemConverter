using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class History
    {
        private Stack<Record> records;


        public void AddRecord(Record newRecord)
        {
            records.Push(newRecord);
        }
        public Record PopRecord()
        {
            return records.Pop();
        }
        public void Clear()
        {
            records.Clear();
        }


        public int Count => records.Count;
    }

    public struct Record
    {
        public string sourceNumber;
        public string resultNumber;
        public int sourceRadix;
        public int resultRadix;


        public Record(string sourceNumber, string resultNumber, int sourceRadix, int resultRadix)
        {
            this.sourceNumber = sourceNumber;
            this.resultNumber = resultNumber;
            this.sourceRadix = sourceRadix;
            this.resultRadix = resultRadix;
        }


        public override string ToString()
        {
            return sourceNumber + " [" + sourceRadix + "] -> " + resultNumber + " [" + resultRadix + "]" + Environment.NewLine;
        }
    }
}
