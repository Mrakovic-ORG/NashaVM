using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Ldloc : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Ldloc;

        public OpCode[] Inputs => new[] { OpCodes.Ldloc, OpCodes.Ldloc_S, OpCodes.Ldloc_0, OpCodes.Ldloc_1, OpCodes.Ldloc_2, OpCodes.Ldloc_3 };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Ldloc, method.Body.Variables.IndexOf(method.Body.Instructions[index].GetLocal(method.Body.Variables)));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[5];

            buf[0] = (byte)NashaOpcodes.Ldloc.ShuffledID;
            Array.Copy(BitConverter.GetBytes((int)instruction.Operand), 0, buf, 1, 4);
            return buf;
        }
    }
}
