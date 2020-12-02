using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Mul : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Mul;

        public OpCode[] Inputs => new[] { OpCodes.Mul, OpCodes.Mul_Ovf, OpCodes.Mul_Ovf_Un };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Mul);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Mul.ShuffledID };
        }
    }
}
