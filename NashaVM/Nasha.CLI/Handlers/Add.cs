using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Add : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Add;

        public OpCode[] Inputs => new[] { OpCodes.Add, OpCodes.Add_Ovf, OpCodes.Add_Ovf_Un };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Add);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Add.ShuffledID };
        }
    }
}
