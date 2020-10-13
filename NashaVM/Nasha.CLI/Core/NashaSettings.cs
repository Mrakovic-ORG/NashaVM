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
        //public List<>
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

        public List<byte> TranslateReference()
        {
            var arr = new List<byte>();

            arr.AddRange(BitConverter.GetBytes(References.Count));
            for (int i = 0; i < References.Count; ++i)
            {
                arr.AddRange(BitConverter.GetBytes(References[i].Length));
                arr.AddRange(Encoding.UTF8.GetBytes(References[i]));
            }

            return arr;
        }

        public List<byte> TranslateOpcodes()
        {
            var arr = new List<byte>();

            int Br = 0;
            int Push = 1;
            int Exit = 2;
            int SetBlock = 4;
            int Nothing = 5;

            var list = NashaOpcodes.OpcodesList().ToList();
            var blocks = new List<OpcodesBlock>();

            for (int i = 0; i < list.Count; ++i)
            {
                var stub = new List<byte>();
                stub.AddRange(BitConverter.GetBytes(SetBlock));
                stub.AddRange(BitConverter.GetBytes(list[i].ID));

                stub.AddRange(BitConverter.GetBytes(Push));
                stub.AddRange(BitConverter.GetBytes(list[i].ShuffledID));

                stub.AddRange(BitConverter.GetBytes(Br));
                try
                {
                    stub.AddRange(BitConverter.GetBytes(list[i + 1].ID));
                }
                catch 
                {
                    stub.AddRange(BitConverter.GetBytes(777)); // Exit control flow.
                }

                blocks.Add(new OpcodesBlock(list[i].ID, stub.ToArray()));
            }

            arr.AddRange(BitConverter.GetBytes(Br));
            arr.AddRange(BitConverter.GetBytes(blocks[0].ID));

            var rndBlocks = blocks.ToList().OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var block in rndBlocks)
                arr.AddRange(block.Content);

            arr.AddRange(BitConverter.GetBytes(SetBlock));
            arr.AddRange(BitConverter.GetBytes(777));

            arr.AddRange(BitConverter.GetBytes(Exit));
            arr.AddRange(BitConverter.GetBytes(Nothing));

            return arr;
        }
    }
}
