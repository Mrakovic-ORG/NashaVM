using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Newarr : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Newarr;

        public OpCode[] Inputs => new[] { OpCodes.Newarr };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Newarr);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Newarr.ShuffledID };
        }
    }
}
