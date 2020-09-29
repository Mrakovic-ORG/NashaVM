using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Ret : IHandler
    {
        public NashaOpcode Handler => NashaOpcode.Ret;

        public OpCode[] Inputs => new[] { OpCodes.Ret };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcode.Ret);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcode.Ret };
        }

    }
}
