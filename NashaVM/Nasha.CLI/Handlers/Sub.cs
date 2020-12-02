using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Sub : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Sub;

        public OpCode[] Inputs => new[] { OpCodes.Sub, OpCodes.Sub_Ovf, OpCodes.Sub_Ovf_Un };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Sub);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Sub.ShuffledID };
        }
    }
}
