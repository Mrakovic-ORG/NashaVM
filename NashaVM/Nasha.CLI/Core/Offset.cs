using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public readonly struct Offset
    {
        public Offset(int start, int val)
        {
            Start = start;
            Value = val;
        }

        public readonly int Start;
        public readonly int Value;
    }
}
