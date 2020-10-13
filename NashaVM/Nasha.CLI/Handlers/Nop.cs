using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Nop : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Nop;

        public OpCode[] Inputs => new[] { OpCodes.Nop };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Nop);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Nop.ShuffledID };
        }
    }
}
