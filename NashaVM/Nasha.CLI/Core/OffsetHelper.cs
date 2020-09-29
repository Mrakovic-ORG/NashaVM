using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class OffsetHelper
    {
        private static List<Offset> _offsets = new List<Offset>();

        public static void Add(int index, int offset)
        {
            _offsets.Add(new Offset(index, offset));
        }

        public static int Get(int index)
        {
            return _offsets.Where(o => o.Starts < index).Sum(off => off.Value) - 1 + index;
        }
    }
}
