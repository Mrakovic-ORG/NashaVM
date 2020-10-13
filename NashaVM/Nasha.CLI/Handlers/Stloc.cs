using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Stloc : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Stloc;

        public OpCode[] Inputs => new[] { OpCodes.Stloc, OpCodes.Stloc_S, OpCodes.Stloc_0, OpCodes.Stloc_1, OpCodes.Stloc_2, OpCodes.Stloc_3 };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Stloc, method.Body.Variables.IndexOf(method.Body.Instructions[index].GetLocal(method.Body.Variables)));

        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[5];

            buf[0] = (byte)NashaOpcodes.Stloc.ShuffledID;
            Array.Copy(BitConverter.GetBytes((int)instruction.Operand), 0, buf, 1, 4);
            return buf;
        }
    }
}
