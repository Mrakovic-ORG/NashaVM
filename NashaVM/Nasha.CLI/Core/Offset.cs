using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public struct Offset
    {
        public Offset(int start, int val)
        {
            Starts = start;
            Value = val;
        }

        public int Starts;
        public int Value;
    }
}
