namespace Nasha.CLI.Core {
    public class NashaInstruction {
        public NashaOpcode OpCode { get; }
        public object Operand { get; }

        public NashaInstruction(NashaOpcode opcode, object operand = null) => 
            (OpCode, Operand) = (opcode, operand);
    }
}