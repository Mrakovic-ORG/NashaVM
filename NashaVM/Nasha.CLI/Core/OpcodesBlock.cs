using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class OpcodesBlock
    {
        public int Identifier { get; private set; }
        public byte[] Content { get; private set; }
        public OpcodesBlock(int identifier, byte[] blockContent)
        {
            Identifier = identifier;
            Content = blockContent;
        }
    }
}
