using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class Translated
    {
        public MethodDef Method { get; }
        public List<NashaInstruction> Instructions { get; }

        public Translated(MethodDef method, List<NashaInstruction> instructions)
        {
            Method = method;
            Instructions = instructions;
        }
    }
}
