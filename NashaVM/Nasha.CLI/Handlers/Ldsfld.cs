﻿using System;
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

            return new NashaInstruction(NashaOpcodes.Ldsfld, new Tuple<short, IField>((short)settings.References.IndexOf(assemblyName), field));
        }

        public byte[] Serializer(NashaSettings settings, NashaInstruction instruction)
        {
            var buf = new byte[7];

            buf[0] = (byte)NashaOpcodes.Ldsfld.ShuffledID;
            var (referenceId, field) = (Tuple<short, IField>)instruction.Operand;
            Array.Copy(BitConverter.GetBytes(referenceId), 0, buf, 1, 2);
            Array.Copy(BitConverter.GetBytes(TokenGetter.GetFieldToken(field)), 0, buf, 3, 4);
            return buf;
        }
    }
}
