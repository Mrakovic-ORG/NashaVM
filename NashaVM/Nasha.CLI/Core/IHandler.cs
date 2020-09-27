using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace Nasha.CLI.Core
{
    public interface IHandler
    {
        NashaOpcode Handler { get; }
        OpCode[] Inputs { get; }
        NashaInstruction Translation(NashaSettings settings, MethodDef method, int index);
        byte[] Serializer(NashaSettings settings, NashaInstruction instruction);
    }
}
