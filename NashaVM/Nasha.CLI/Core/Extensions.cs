using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class Extensions
    {
        public static NashaOpcode Lookup(this List<NashaOpcode> Opcodes, string Find)
        {
            return Opcodes.FirstOrDefault(opcode => opcode.Name == Find);
        }

        public static Colorful.Formatter ToColor(this string text, System.Drawing.Color color) => new Colorful.Formatter(text, color);

        public static void WriteLineFormatted(string text, params Colorful.Formatter[] args)
        {
            Colorful.Console.WriteFormatted(text, Color.LightGray, args);
        }

        public static void WriteLineFormatted(string text, Color baseColor, params Colorful.Formatter[] args)
        {
            Colorful.Console.WriteFormatted(text, baseColor, args);
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
