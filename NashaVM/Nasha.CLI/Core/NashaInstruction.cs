using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public class NashaInstruction
    {
        public NashaOpcode OpCode { get; }
        public object Operand { get; }

        public NashaInstruction(NashaOpcode opcode, object operand = null)
        {
            OpCode = opcode;
            Operand = operand;
        }
    }
}
