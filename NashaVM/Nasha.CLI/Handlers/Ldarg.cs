using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;

namespace Nasha.CLI.Handlers
{
    public class Ldarg : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Ldarg;

        public OpCode[] Inputs => new[] { OpCodes.Ldarg, OpCodes.Ldarg_0, OpCodes.Ldarg_1, OpCodes.Ldarg_2, OpCodes.Ldarg_3, OpCodes.Ldarg_S };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Ldarg, (short)method.Body.Instructions[index].GetParameterIndex());
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[3];
            buf[0] = (byte)NashaOpcodes.Ldarg.ShuffledID;
            Array.Copy(BitConverter.GetBytes((short)instruction.Operand), 0, buf, 1, 2);
            return buf;
        }
    }
}
