using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Brtrue : IHandler
    {
        public NashaOpcode Handler => NashaOpcode.Brtrue;

        public OpCode[] Inputs => new[] { OpCodes.Brtrue, OpCodes.Brtrue_S };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcode.Brtrue, OffsetHelper.Get(method.Body.Instructions.IndexOf((Instruction)method.Body.Instructions[index].Operand)));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[5];

            buf[0] = (byte)NashaOpcode.Brtrue;
            Array.Copy(BitConverter.GetBytes((int)instruction.Operand), 0, buf, 1, 4);
            return buf;
        }
    }
}
