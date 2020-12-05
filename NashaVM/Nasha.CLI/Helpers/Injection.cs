using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Helpers
{
    public static class Injection
    {
        public static IMethod InjectRuntimeMethod(ModuleDefMD RuntimeModule, ModuleDefMD TargetModule, string Type, string Method)
        {
            return TargetModule.Import(RuntimeModule.Types.ToArray().First(x => x.Name == Type).Methods.First(x => x.Name == Method));
        }
    }
}
