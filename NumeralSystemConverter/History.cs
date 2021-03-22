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
        public string expression;


        public Record(string expression)
        {
            this.expression = expression;
        }


        public override string ToString()
        {
            return expression + Environment.NewLine;
        }
    }
}
