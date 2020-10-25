using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class OffsetHelper
    {
        private static readonly List<Offset> Offsets = new List<Offset>();

        public static void Add(int index, int offset)
        {
            Offsets.Add(new Offset(index, offset));
        }

        public static int Get(int index)
        {
            return Offsets.Where(o => o.Start < index).Sum(off => off.Value) - 1 + index;
        }
    }
}
