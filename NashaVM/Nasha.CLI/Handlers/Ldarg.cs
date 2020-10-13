using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Ldarg : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Ldarg;

        public OpCode[] Inputs => new[] { OpCodes.Ldarg, OpCodes.Ldarg_0, OpCodes.Ldarg_1, OpCodes.Ldarg_2, OpCodes.Ldarg_3, OpCodes.Ldarg_S };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            var arg = method.Body.Instructions[index].GetParameterIndex();
            return new NashaInstruction(NashaOpcodes.Ldarg, arg);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            return new[] { (byte)NashaOpcodes.Ldarg.ShuffledID };
        }
    }
}
