using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Pop : IHandler
    {
        public NashaOpcode Handler => NashaOpcode.Pop;

        public OpCode[] Inputs => new[] { OpCodes.Pop };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcode.Pop);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcode.Pop };
        }
    }
}
