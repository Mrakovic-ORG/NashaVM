using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class OpcodesBlock
    {
        public int ID { get; set; }
        public byte[] Content { get; set; }
        public OpcodesBlock(int ID, byte[] Content)
        {
            this.ID = ID;
            this.Content = Content;
        }
    }
}
