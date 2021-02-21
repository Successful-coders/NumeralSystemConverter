using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter
{
    class History
    {
        private List<Record> records = new List<Record>();


        public void AddRecord(Record newRecord)
        {
            records.Add(newRecord);
        }
        public Record this[int i]
        {
            get
            {
                return records.ElementAt(i);
            }
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
