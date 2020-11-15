using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;

namespace Nasha.CLI.Handlers
{
    public class LdcR8 : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.LdcR8;

        public OpCode[] Inputs => new[] { OpCodes.Ldc_R8 };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.LdcR8, (double)method.Body.Instructions[index].Operand);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[9];
            buf[0] = (byte)NashaOpcodes.LdcR8.ShuffledID;
            Array.Copy(BitConverter.GetBytes((double)instruction.Operand), 0, buf, 1, 8);
            return buf;
        }
    }
}