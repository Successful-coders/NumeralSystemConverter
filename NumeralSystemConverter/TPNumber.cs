using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumeralSystemConverter.Converter;

namespace NumeralSystemConverter
{
    class TPNumber
    {
        public const int MinP = 2;
        public const int MaxP = 16;

        public static string zero = "0";

        public string number;
        public int p, c;


        public TPNumber(double a, int p, int c)
        {
            if (p < MinP || p > MaxP)
                return;
            this.p = p;
            number = ConverterFrom10.Convert(a, p, c);
            this.c = c;

        }
        public TPNumber(string a, string p, string c)
        {
            if (Convert.ToInt16(p) < Convert.ToInt16(MinP) || Convert.ToInt16(p) > Convert.ToInt16(MaxP))
                return;

            this.p = Convert.ToInt16(p);
            this.c = Convert.ToInt16(c);
            number = a;
        }
        public TPNumber() { }

        public TPNumber Copy()
        {
            TPNumber tmp = new TPNumber(this.number, Convert.ToString(this.p), Convert.ToString(this.c));
            return tmp;
        }
        public TPNumber Add(TPNumber d)
        {
            TPNumber result = new TPNumber();
            double sum = (ConverterTo10.Convert(Convert.ToString(d.number), d.p)
                + ConverterTo10.Convert(Convert.ToString(this.number), p));
            result.number = Convert.ToString(ConverterFrom10.Convert(sum, d.p, d.c));
            result.p = d.p;
            result.c = d.c > this.c ? d.c : this.c;
            return result;
        }
        public TPNumber Multiply(TPNumber d)
        {
            TPNumber result = new TPNumber();
            result.number = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(d.number), d.p)
                * ConverterTo10.Convert(Convert.ToString(this.number), p), d.p, d.c));
            result.p = d.p;
            result.c = d.c + this.c;
            return result;
        }
        public TPNumber Substract(TPNumber d)
        {
            TPNumber result = new TPNumber();
            result.number = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(this.number), p) -
                ConverterTo10.Convert(Convert.ToString(d.number), d.p), d.p, d.c));
            result.p = d.p;
            result.c = d.c > this.c ? d.c : this.c;
            return result;
        }
        public TPNumber Divide(TPNumber d)
        {
            TPNumber result = new TPNumber();
            result.number = Convert.ToString(ConverterFrom10.Convert(ConverterTo10.Convert(Convert.ToString(this.number), p) /
                ConverterTo10.Convert(Convert.ToString(d.number), d.p), d.p, d.c));
            result.p = d.p;
            result.c = d.c > this.c ? d.c : this.c;
            return result;
        }
        public TPNumber Inverse()
        {
            if (c == 0)
                c = 15;
            else
                c = 15;
            TPNumber result = new TPNumber(number, Convert.ToString(p), Convert.ToString(c));
            result.number = Convert.ToString(ConverterFrom10.Convert(Math.Round(1 / ConverterTo10.Convert(number, p), c), p, c));
            return result;
        }
        public TPNumber Square()
        {
            if (c < 8)
                c = c * 2;
            else
                c = 15;
            TPNumber result = new TPNumber(number, Convert.ToString(p), Convert.ToString(c));
            result.number = Convert.ToString(ConverterFrom10.Convert(Math.Round(Math.Pow(ConverterTo10.Convert(number, p), 2), c), p, c));
            return result;
        }


        public double PNum => ConverterTo10.Convert(number, p);
        public double NumberP => p;
        public string StringP
        {
            get
            {
                return number;
            }
            set
            {
                p = Convert.ToInt16(value);
            }
        }
        public double NumberC => c;
        public string StringC
        {
            get
            {
                return c.ToString();
            }
            set
            {
                c = Convert.ToInt16(value);
            }
        }
        public int P { set => p = value; }
        public int C { set => c = value; }
    }
}
