using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Ret : IHandler
    {
        public NashaOpcode Handler => NashaOpcode.Ret;

        public OpCode[] Inputs => new[] { OpCodes.Ret };

        public NashaInstruction Translation(NashaSettings body, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcode.Ret);
        }

        public byte[] Serializer(NashaSettings body, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcode.Ret };
        }

    }
}
