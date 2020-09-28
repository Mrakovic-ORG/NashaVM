using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace Nasha.CLI.Core
{
    internal static class TokenGetter
    {
        internal static ModuleWriterBase Writer;

        internal static int GetMdToken(IMethod member)
        {
            return Writer.Module == member.Module
                ? Writer.Metadata.GetToken(member).ToInt32()
                : member.MDToken.ToInt32();
        }
    }
}
