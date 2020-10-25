using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class OpcodesBlock
    {
        public int ID { get; private set; }
        public byte[] Content { get; private set; }
        public OpcodesBlock(int ID, byte[] Content)
        {
            this.ID = ID;
            this.Content = Content;
        }
    }
}
