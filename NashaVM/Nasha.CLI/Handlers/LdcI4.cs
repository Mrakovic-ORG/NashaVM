using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;

namespace Nasha.CLI.Handlers
{
    public class LdcI4 : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.LdcI4;

        public OpCode[] Inputs => new[] { OpCodes.Ldc_I4, OpCodes.Ldc_I4_S, OpCodes.Ldc_I4_0, OpCodes.Ldc_I4_1, OpCodes.Ldc_I4_2, OpCodes.Ldc_I4_3, OpCodes.Ldc_I4_4, OpCodes.Ldc_I4_5, OpCodes.Ldc_I4_6, OpCodes.Ldc_I4_7, OpCodes.Ldc_I4_8 };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.LdcI4, method.Body.Instructions[index].GetLdcI4Value());
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[5];
            buf[0] = (byte)NashaOpcodes.LdcI4.ShuffledID;
            Array.Copy(BitConverter.GetBytes((int)instruction.Operand), 0, buf, 1, 4);
            return buf;
        }

    }
}
