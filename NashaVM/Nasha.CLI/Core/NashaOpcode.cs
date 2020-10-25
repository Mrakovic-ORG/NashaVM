using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class NashaOpcode
    {
        public string Name { get; private set; }
        public int ID { get; private set; }
        public int ShuffledID { get; private set; }

        public NashaOpcode(int ID)
        {
            this.ID = ID;
            this.ShuffledID = NashaOpcodes.GenerateInteger();
        }

        public NashaOpcode(string Name, int ID)
        {
            this.Name = Name;
            this.ID = ID;
            this.ShuffledID = NashaOpcodes.GenerateInteger();
        }
    }

    public static class NashaOpcodes
    {
        public static NashaOpcode Ret = new NashaOpcode(0);
        public static NashaOpcode LdcI4 = new NashaOpcode(1);
        public static NashaOpcode Ldstr = new NashaOpcode(2);
        public static NashaOpcode Call = new NashaOpcode(3);
        public static NashaOpcode Nop = new NashaOpcode(4);
        public static NashaOpcode Pop = new NashaOpcode(5);
        public static NashaOpcode Ldsfld = new NashaOpcode(6);
        public static NashaOpcode Stsfld = new NashaOpcode(7);
        public static NashaOpcode Brfalse = new NashaOpcode(8);
        public static NashaOpcode Brtrue = new NashaOpcode(9);
        public static NashaOpcode Br = new NashaOpcode(10);
        public static NashaOpcode Ldloc = new NashaOpcode(11);
        public static NashaOpcode Stloc = new NashaOpcode(12);
        public static NashaOpcode Ldarg = new NashaOpcode(13);
        public static NashaOpcode Newarr = new NashaOpcode(14);
        public static NashaOpcode Castclass = new NashaOpcode(15);
        public static NashaOpcode Newobj = new NashaOpcode(16);
        public static NashaOpcode Dup = new NashaOpcode(17);
        public static NashaOpcode Ldftn = new NashaOpcode(18);

        private static int _generatorOffset = 0;

        public static int GenerateInteger()
        {
            var x = RandomNumbers(255);
            return x[_generatorOffset++];
        }

        private static int[] RandomNumbers(int rangeEx)
        {
            var orderedList = Enumerable.Range(1, rangeEx);
            var rng = new Random();
            var numbers = orderedList.OrderBy(c => rng.Next()).ToArray();
            numbers.Shuffle();
            return numbers;
        }

        private static readonly List<NashaOpcode> List = new List<NashaOpcode>();

        public static List<NashaOpcode> OpcodesList()
        {
            if (List.Count >= 2) return List;

            List.Add(Ret);
            List.Add(LdcI4);
            List.Add(Ldstr);
            List.Add(Call);
            List.Add(Nop);
            List.Add(Pop);
            List.Add(Ldsfld);
            List.Add(Stsfld);
            List.Add(Brfalse);
            List.Add(Brtrue);
            List.Add(Br);
            List.Add(Ldloc);
            List.Add(Stloc);
            List.Add(Ldarg);
            List.Add(Newarr);
            List.Add(Castclass);
            List.Add(Newobj);
            List.Add(Dup);
            List.Add(Ldftn);
            return List;
        }
    }
}