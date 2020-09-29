using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public enum NashaOpcode : byte
    {
        Ret = 0,
        LdcI4 = 1,
        Ldstr = 2,
        Call = 3,
        Nop = 4,
        Pop = 5,
        Ldsfld = 6,
        Stsfld = 7,
        Brfalse = 8,
        Brtrue = 9,
        Br = 10,
        Ldloc = 11,
        Stloc = 12
    }
}
