using dnlib.DotNet;
using dnlib.DotNet.Writer;

namespace Nasha.CLI.Core
{
    internal static class TokenGetter
    {
        internal static ModuleWriterBase Writer;

        internal static int GetMdToken(IMethod member)
        {
            return Writer.Module == member.Module ? Writer.Metadata.GetToken(member).ToInt32() : member.MDToken.ToInt32();
        }
        internal static int GetFieldToken(IField field)
        {
            return Writer.Module == field.Module ? Writer.Metadata.GetToken(field).ToInt32() : field.MDToken.ToInt32();
        }
        internal static int GetFieldToken(GenericSig field)
        {
            return Writer.Module == field.Module ? Writer.Metadata.GetToken(field).ToInt32() : field.MDToken.ToInt32();
        }
        internal static int GetGenericSigToken(GenericSig field)
        {
            return Writer.Module == field.Module ? Writer.Metadata.GetToken(field).ToInt32() : field.MDToken.ToInt32();
        }
        internal static int GetTypeToken(ITypeDefOrRef type)
        {
            return Writer.Module == type.Module ? Writer.Metadata.GetToken(type).ToInt32() : type.MDToken.ToInt32();
        }
    }
}
