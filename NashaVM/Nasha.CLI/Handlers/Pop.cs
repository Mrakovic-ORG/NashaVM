using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Pop : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Pop;

        public OpCode[] Inputs => new[] { OpCodes.Pop };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Pop);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Pop.ShuffledID };
        }
    }
}
