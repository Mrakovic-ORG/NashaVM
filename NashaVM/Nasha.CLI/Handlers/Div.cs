using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Div : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Div;

        public OpCode[] Inputs => new[] { OpCodes.Div, OpCodes.Div_Un };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Div);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Div.ShuffledID };
        }
    }
}
