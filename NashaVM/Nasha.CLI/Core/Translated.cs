using dnlib.DotNet; 
using System.Collections.Generic; 

namespace Nasha.CLI.Core {
    public class Translated {
        public MethodDef Method { get; }
        public List<NashaInstruction> Instructions { get; } 
        public Translated(MethodDef method, List<NashaInstruction> instructions) => 
            (Method, Instructions) = (method, instructions);
    }
}