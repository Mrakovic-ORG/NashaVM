using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class Map
    {
        private static readonly Dictionary<OpCode, IHandler> OpCodeToHandler = new Dictionary<OpCode, IHandler>();
        private static readonly Dictionary<NashaOpcode, IHandler> VMOpCodeToHandler = new Dictionary<NashaOpcode, IHandler>();

        static Map()
        {
            foreach (var type in typeof(Map).Assembly.DefinedTypes)
            {
                if (type.IsInterface || !typeof(IHandler).IsAssignableFrom(type))
                    continue;

                var instance = (IHandler)Activator.CreateInstance(type);

                foreach (var opcode in instance.Inputs)
                    OpCodeToHandler.Add(opcode, instance);

                VMOpCodeToHandler.Add(instance.Handler, instance);
            }
        }

        public static IHandler Lookup(OpCode opcode)
        {
            return OpCodeToHandler.ContainsKey(opcode) ? OpCodeToHandler[opcode] : null;
        }

        public static IHandler Lookup(NashaOpcode opcode)
        {
            return VMOpCodeToHandler.ContainsKey(opcode) ? VMOpCodeToHandler[opcode] : null;
        }
    }
}
