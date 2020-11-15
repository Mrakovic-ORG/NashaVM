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
        public readonly List<Translated> Translated;

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
            foreach (var reference in References)
            {
                arr.AddRange(BitConverter.GetBytes(reference.Length));
                arr.AddRange(Encoding.UTF8.GetBytes(reference));
            }

            return arr;
        }

        static Random random = new Random();
        public static List<byte> TranslateOpcodes()
        {
            var arr = new List<byte>();

            int Br = 0;
            int Push = 1;
            int Exit = 2;
            int SetBlock = 4;
            int Nothing = 5;
            int ExitBlock = random.Next(256, 1337);

            var list = NashaOpcodes.OpcodesList().ToList();
            var blocks = new List<OpcodesBlock>();

            for (int i = 0; i < list.Count; ++i)
            {
                var stub = new List<byte>();
                stub.AddRange(BitConverter.GetBytes(SetBlock));
                stub.AddRange(BitConverter.GetBytes(list[i].BlockID));

                if (random.Next(0, 2) == 1)
                {
                    stub.AddRange(BitConverter.GetBytes(random.Next(5, 10)));
                    stub.AddRange(BitConverter.GetBytes(random.Next(5, 10)));
                }

                stub.AddRange(BitConverter.GetBytes(Push));
                stub.AddRange(BitConverter.GetBytes(list[i].ShuffledID));

                stub.AddRange(BitConverter.GetBytes(Br));
                try
                {
                    stub.AddRange(BitConverter.GetBytes(list[i + 1].BlockID));
                }
                catch 
                {
                    stub.AddRange(BitConverter.GetBytes(ExitBlock)); // Exit control flow.
                }

                blocks.Add(new OpcodesBlock(list[i].BlockID, stub.ToArray()));
            }

            arr.AddRange(BitConverter.GetBytes(Br));
            arr.AddRange(BitConverter.GetBytes(blocks[0].ID));

            var rndBlocks = blocks.ToList().OrderBy(x => Guid.NewGuid()).ToList();
            foreach (var block in rndBlocks)
                arr.AddRange(block.Content);

            arr.AddRange(BitConverter.GetBytes(SetBlock));
            arr.AddRange(BitConverter.GetBytes(ExitBlock));

            arr.AddRange(BitConverter.GetBytes(Exit));
            arr.AddRange(BitConverter.GetBytes(Nothing));

            return arr;
        }
    }
}
