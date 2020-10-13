using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class Extensions
    {
        public static NashaOpcode Lookup(this List<NashaOpcode> Opcodes, string Find)
        {
            foreach (var Opcode in Opcodes)
                if (Opcode.Name == Find)
                    return Opcode;
            return null;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
