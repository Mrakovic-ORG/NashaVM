namespace Nasha.CLI.Core {
    public readonly struct Offset {
        public Offset(int start, int value) {
            Start = start;
            Value = value;
        }

        public readonly int Start;
        public readonly int Value;
    }
}
