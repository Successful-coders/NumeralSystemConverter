using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeralSystemConverter.Converter
{
    public class Constants
    {
        internal const int MIN_RADIX = 2;
        internal const int MAX_RADIX = 16;

        internal static char[] baseSymbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
    }
}
