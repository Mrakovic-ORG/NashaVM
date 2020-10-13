using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Brfalse : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Brfalse;

        public OpCode[] Inputs => new[] { OpCodes.Brfalse, OpCodes.Brfalse_S };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Brfalse, OffsetHelper.Get(method.Body.Instructions.IndexOf((Instruction)method.Body.Instructions[index].Operand)));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[5];

            buf[0] = (byte)NashaOpcodes.Brfalse.ShuffledID;
            Array.Copy(BitConverter.GetBytes((int)instruction.Operand), 0, buf, 1, 4);
            return buf;
        }
    }
}
