using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Nop : IHandler
    {
        public NashaOpcode Handler => NashaOpcode.Nop;

        public OpCode[] Inputs => new[] { OpCodes.Nop };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcode.Nop);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcode.Nop };
        }
    }
}
