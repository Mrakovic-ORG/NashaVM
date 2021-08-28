using System;
using System.Collections.Generic;
using System.Linq; 

namespace Nasha.CLI.Core {
    public class NashaOpcode { 
        public int Identifier { get; private set; }
        public int ShuffledIdentifier { get; private set; }
        public int BlockIdentifier { get; private set; }

        public NashaOpcode(int identifier)
        {
            this.Identifier = identifier;
            this.ShuffledIdentifier = NashaOpcodes.GenerateInteger();
            this.BlockIdentifier = NashaOpcodes.GenerateBlockInteger();
        } 
    }

    public static class NashaOpcodes {
        
        // Flow Control.
        public static NashaOpcode Brfalse = new NashaOpcode(8);
        public static NashaOpcode Brtrue = new NashaOpcode(9);
        public static NashaOpcode Br = new NashaOpcode(10);
        public static NashaOpcode Ret = new NashaOpcode(0);
        
        // Invocation.
        public static NashaOpcode Call = new NashaOpcode(3);
        public static NashaOpcode Newarr = new NashaOpcode(14);
        public static NashaOpcode Castclass = new NashaOpcode(15);
        public static NashaOpcode Newobj = new NashaOpcode(16);
        public static NashaOpcode Ldftn = new NashaOpcode(18);

        // Fields.
        public static NashaOpcode Ldsfld = new NashaOpcode(6);
        public static NashaOpcode Stsfld = new NashaOpcode(7);
        
        // Arguments/Locals.
        public static NashaOpcode Ldloc = new NashaOpcode(11);
        public static NashaOpcode Stloc = new NashaOpcode(12);
        public static NashaOpcode Ldarg = new NashaOpcode(13); 

        // Misc.
        public static NashaOpcode Nop = new NashaOpcode(4);
        public static NashaOpcode Pop = new NashaOpcode(5);
        public static NashaOpcode Dup = new NashaOpcode(17); 

        // Constants.
        public static NashaOpcode LdcI4 = new NashaOpcode(1);
        public static NashaOpcode Ldstr = new NashaOpcode(2);
        public static NashaOpcode LdcR8 = new NashaOpcode(19);

        // Arithmetics.
        public static NashaOpcode Add = new NashaOpcode(20);
        public static NashaOpcode Sub = new NashaOpcode(21);
        public static NashaOpcode Mul = new NashaOpcode(22);
        public static NashaOpcode Div = new NashaOpcode(23);
        public static NashaOpcode Xor = new NashaOpcode(24);

        private static int _generatorOffset = 0;
        private static int _generatorBlockOffset = 0;

        static List<int> _generator1;
        public static int GenerateBlockInteger()
        {
            if (_generator1 == null)
                _generator1 = UniqueRandom(0, 255).ToList();
            return _generator1[_generatorBlockOffset++];
        }

        static List<int> _generator0;
        public static int GenerateInteger()
        {
            if (_generator0 == null)
                _generator0 = UniqueRandom(0, 255).ToList();
            return _generator0[_generatorOffset++];
        }

        private static int tickcount;
        private static IEnumerable<int> UniqueRandom(int minInclusive, int maxInclusive)
        {
            List<int> candidates = new List<int>();
            for (int i = minInclusive; i <= maxInclusive; i++)
            {
                candidates.Add(i);
            }
            if (tickcount == 0)
                tickcount = Environment.TickCount;
            Random rnd = new Random(tickcount);
            tickcount *= 2;
            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                yield return candidates[index];
                candidates.RemoveAt(index);
            }
        }

        private static readonly List<NashaOpcode> List = new List<NashaOpcode>();

        public static List<NashaOpcode> OpcodesList()
        {
            if (List.Count >= 1)
                return List;



            var nashaCodes = typeof(NashaOpcodes)
                .GetFields()
                .Where(x => x.IsStatic && x.FieldType == typeof(NashaOpcode))
                .Select(x => (NashaOpcode)x.GetValue(null))
                .OrderBy(x => x.Identifier);

            List.AddRange(nashaCodes);


            return List;
        }
    }
}