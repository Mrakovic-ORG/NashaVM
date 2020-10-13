using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;
using System.Text;

namespace Nasha.CLI.Handlers
{
    public class Ldstr : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Ldstr;

        public OpCode[] Inputs => new[] { OpCodes.Ldstr };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            return new NashaInstruction(NashaOpcodes.Ldstr, method.Body.Instructions[index].Operand);
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var str = Encoding.UTF8.GetBytes(instruction.Operand.ToString());

            var buf = new byte[5 + str.Length];
            buf[0] = (byte)NashaOpcodes.Ldstr.ShuffledID;
            Array.Copy(BitConverter.GetBytes(str.Length), 0, buf, 1, 4);
            Array.Copy(str, 0, buf, 5, str.Length);
            return buf;
        }
    }
}
