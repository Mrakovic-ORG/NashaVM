using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class NashaSettings
    {
        public List<string> References { get; }
        public List<Translated> Translated;

        public NashaSettings()
        {
            References = new List<string>();
            Translated = new List<Translated>();
        }

        public List<byte> Serialize(Translated translated)
        {
            var arr = new List<byte>();

            arr.AddRange(BitConverter.GetBytes(translated.Instructions.Count));
            foreach (var instruction in translated.Instructions)
                arr.AddRange(Map.Lookup(instruction.OpCode).Serializer(this, instruction));

            return arr;
        }
    }
}
