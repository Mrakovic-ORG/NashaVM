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
        public static NashaOpcode Ret = new("Ret", 0);
        public static NashaOpcode LdcI4 = new(1);
        public static NashaOpcode Ldstr = new(2);
        public static NashaOpcode Call = new(3);
        public static NashaOpcode Nop = new(4);
        public static NashaOpcode Pop = new(5);
        public static NashaOpcode Ldsfld = new(6);
        public static NashaOpcode Stsfld = new(7);
        public static NashaOpcode Brfalse = new(8);
        public static NashaOpcode Brtrue = new(9);
        public static NashaOpcode Br = new(10);
        public static NashaOpcode Ldloc = new(11);
        public static NashaOpcode Stloc = new(12);
        public static NashaOpcode Ldarg = new(13);
        public static NashaOpcode Newarr = new(14);

        private static int generatorOffset = 0;
        public static int GenerateInteger()
        {
            var x = RandomNumbers(255);
            return x[generatorOffset++];
        }

        private static int[] RandomNumbers(int rangeEx)
        {
            var orderedList = Enumerable.Range(1, rangeEx);
            var rng = new Random();
            var numbers = orderedList.OrderBy(c => rng.Next()).ToArray();
            numbers.Shuffle();
            return numbers;
        }

        private static List<NashaOpcode> _list = new List<NashaOpcode>();

        public static List<NashaOpcode> OpcodesList()
        {
            if (_list.Count < 2)
            {
                _list.Add(Ret);
                _list.Add(LdcI4);
                _list.Add(Ldstr);
                _list.Add(Call);
                _list.Add(Nop);
                _list.Add(Pop);
                _list.Add(Ldsfld);
                _list.Add(Stsfld);
                _list.Add(Brfalse);
                _list.Add(Brtrue);
                _list.Add(Br);
                _list.Add(Ldloc);
                _list.Add(Stloc);
                _list.Add(Ldarg);
            }

            return _list;
        }
    }
}
