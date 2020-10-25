using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using Nasha.CLI.Core;

namespace Nasha.CLI.Handlers
{
    public class Ldsfld : IHandler
    {
        public NashaOpcode Handler => NashaOpcodes.Ldsfld;

        public OpCode[] Inputs => new[] { OpCodes.Ldsfld, OpCodes.Ldfld };

        public NashaInstruction Translation(NashaSettings settings, MethodDef method, int index)
        {
            var field = ((IField)method.Body.Instructions[index].Operand);
            var assemblyName = field.Module.Assembly.FullName;

            if (!settings.References.Contains(assemblyName))
                settings.References.Add(assemblyName);

            return new NashaInstruction(NashaOpcodes.Ldsfld, new Tuple<short, IField, bool>((short)settings.References.IndexOf(assemblyName), field, field.FieldSig.ContainsGenericParameter));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[8];
            buf[0] = (byte)NashaOpcodes.Ldsfld.ShuffledID;

            var (referenceId, field, isGeneric) = (Tuple<short, IField, bool>)instruction.Operand;
            Array.Copy(BitConverter.GetBytes(isGeneric), 0, buf, 1, 1);
            Array.Copy(BitConverter.GetBytes(referenceId), 0, buf, 2, 2);
            Array.Copy(BitConverter.GetBytes(TokenGetter.GetFieldToken(field)), 0, buf, 4, 4);
            return buf;
        }
    }
}
