using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;

namespace Nasha.CLI.Handlers
{
    public class Newobj : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Newobj;

        public OpCode[] Inputs => new[] { OpCodes.Newobj };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            var operand = ((IMethod)method.Body.Instructions[index].Operand);
            var asmName = operand.Module.Assembly.FullName;

            if (!settings.References.Contains(asmName))
                settings.References.Add(asmName);
            return new NashaInstruction(NashaOpcodes.Newobj, new Tuple<short, IMethod>((short)settings.References.IndexOf(asmName), operand));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[7];
            buf[0] = (byte)NashaOpcodes.Newobj.ShuffledID;
            var (referenceId, method) = (Tuple<short, IMethod>)instruction.Operand;
            Array.Copy(BitConverter.GetBytes(referenceId), 0, buf, 1, 2);
            Array.Copy(BitConverter.GetBytes(TokenGetter.GetMdToken(method)), 0, buf, 3, 4);
            return buf;
        }
    }
}
