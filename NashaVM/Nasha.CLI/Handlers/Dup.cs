using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Dup : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Dup;

        public OpCode[] Inputs => new[] { OpCodes.Dup };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Dup);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Dup.ShuffledID };
        }
    }
}
