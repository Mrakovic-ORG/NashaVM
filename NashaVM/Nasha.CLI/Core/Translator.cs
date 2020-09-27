using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasha.CLI.Core
{
    public static class Translator
    {
        public static List<NashaInstruction> Translate(NashaSettings settings, MethodDef method)
        {
            var list = new List<NashaInstruction>();

            for (var i = 0; i < method.Body.Instructions.Count; i++)
            {
                var handler = Map.Lookup(method.Body.Instructions[i].OpCode);
                if (handler == null)
                    return null;

                list.Add(handler.Translation(settings, method, i));
            }
            return list;
        }
        
    }
}
