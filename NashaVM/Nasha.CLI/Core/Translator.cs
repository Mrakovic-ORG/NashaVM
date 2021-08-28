using dnlib.DotNet; 
using System.Collections.Generic; 

namespace Nasha.CLI.Core {
    public static class Translator {
        public static List<NashaInstruction> Translate(NashaSettings settings, MethodDef method) {
            var list = new List<NashaInstruction>();

            for (var i = 0; i < method.Body.Instructions.Count; i++) {
                var handler = Map.Lookup(method.Body.Instructions[i].OpCode);
                if (handler is null)
                    return null;

                list.Add(handler.Translation(settings, method, i));
            }
            return list;
        }
        
    }
}