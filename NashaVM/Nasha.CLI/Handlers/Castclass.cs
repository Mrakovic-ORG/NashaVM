using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;
using System;

namespace Nasha.CLI.Handlers
{
    public class Castclass : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Castclass;

        public OpCode[] Inputs => new[] { OpCodes.Castclass };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            var operand = ((ITypeDefOrRef)method.Body.Instructions[index].Operand);
            var asmName = operand.Module.Assembly.FullName;

            if (!settings.References.Contains(asmName))
                settings.References.Add(asmName);
            return new NashaInstruction(NashaOpcodes.Castclass, new Tuple<short, ITypeDefOrRef>((short)settings.References.IndexOf(asmName), operand));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[7];
            buf[0] = (byte)NashaOpcodes.Castclass.ShuffledID;
            var (referenceId, type) = (Tuple<short, ITypeDefOrRef>)instruction.Operand;
            Array.Copy(BitConverter.GetBytes(referenceId), 0, buf, 1, 2);
            Array.Copy(BitConverter.GetBytes(TokenGetter.GetTypeToken(type)), 0, buf, 3, 4);
            return buf;
        }
    }
}
