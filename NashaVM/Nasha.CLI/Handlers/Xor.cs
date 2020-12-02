using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Xor : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Xor;

        public OpCode[] Inputs => new[] { OpCodes.Xor };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Xor);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Xor.ShuffledID };
        }
    }
}
