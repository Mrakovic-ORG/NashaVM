using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using Nasha.CLI.Core;

namespace Nasha.CLI
{
    internal static class Program
    {
        private static readonly NashaSettings Settings = new NashaSettings();
        private static readonly List<PESection> NashaSections = new List<PESection>();

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Drag'n drop file.");
                Console.ReadKey();
                return;
            }
            var module = ModuleDefMD.Load(args[0]);

            var runtimePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Nasha.dll");
            var runtime = ModuleDefMD.Load(runtimePath);
            IMethod runMethod = runtime.Types.ToArray().First(x => x.Name == "Main").Methods.First(x => x.Name == "Execute");
            IMethod runCtor = runtime.Types.ToArray().First(x => x.Name == "Main").Methods.First(x => x.Name == ".ctor");
            IMethod configCtor = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == ".ctor");
            IMethod configSetup = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == "SetupReferences");
            IMethod configDiscover = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == "SetupDiscover");
            IMethod configVmBytes = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == "SetupBody");

            runMethod = module.Import(runMethod);
            runCtor = module.Import(runCtor);
            configCtor = module.Import(configCtor);
            configSetup = module.Import(configSetup);
            configDiscover = module.Import(configDiscover);
            configVmBytes = module.Import(configVmBytes);

            var configField = new FieldDefUser("cfg", new FieldSig(configCtor.DeclaringType.ToTypeSig()), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            module.GlobalType.Fields.Add(configField);
            var globalConstructor = module.GlobalType.FindOrCreateStaticConstructor();
            globalConstructor.Body.Instructions.Insert(0, OpCodes.Newobj.ToInstruction(configCtor));
            globalConstructor.Body.Instructions.Insert(1, OpCodes.Stsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(2, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(3, OpCodes.Callvirt.ToInstruction(configSetup));
            globalConstructor.Body.Instructions.Insert(4, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(5, OpCodes.Callvirt.ToInstruction(configDiscover));
            globalConstructor.Body.Instructions.Insert(4, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(5, OpCodes.Callvirt.ToInstruction(configVmBytes));

            foreach (var type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                foreach (var method in type.Methods)
                {
                    if (!method.HasBody || !method.Body.HasInstructions) continue;
                    var translated = Translator.Translate(Settings, method);
                    if (translated == null)
                        continue;
                    Settings.Translated.Add(new Translated(method, translated));
                }
            }
            foreach (var translated in Settings.Translated)
            {
                translated.Method.Body = new CilBody() { MaxStack = 1 };
                translated.Method.Body.Instructions.Add(OpCodes.Newobj.ToInstruction(runCtor));
                AddParameters(translated.Method);
                translated.Method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(0));

                translated.Method.Body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(configField));
                translated.Method.Body.Instructions.Add(OpCodes.Call.ToInstruction(runMethod));
                translated.Method.Body.Instructions.Add(translated.Method.HasReturnType ? OpCodes.Unbox_Any.ToInstruction(translated.Method.ReturnType.ToTypeDefOrRef()) : OpCodes.Pop.ToInstruction());

                translated.Method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            }

            var output = Path.GetFileNameWithoutExtension(args[0]) + "-Nasha.exe";
            var writer = new ModuleWriterOptions(module);

            writer.WriterEvent += InsertSections;
            writer.WriterEvent += InsertVmBody;
            writer.MetadataLogger = DummyLogger.NoThrowInstance;
            writer.MetadataOptions.Flags = MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateBlobHeap | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateUSHeap;
            module.Write(output, writer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[#] Virtualized file saved as \"{output}\"");
            Console.ReadKey();
        }

        private static void InsertSections(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;
            if (e.Event == ModuleWriterEvent.MDMemberDefRidsAllocated + 1)
                NashaSections.ForEach(x => writer.AddSection(x));
        }

        private static void InsertVmBody(object sender, ModuleWriterEventArgs e)
        {
            var mainSection = new PESection(".Nasha0", 0x60000020);
            var references = new PESection(".Nasha1", 0x60000020);
            var opcodesList = new PESection(".Nasha2", 0x60000020);

            var writer = (ModuleWriterBase)sender;
            TokenGetter.Writer = writer;
            if (e.Event != ModuleWriterEvent.MDMemberDefRidsAllocated)
                return;

            var translated = Settings.Translated;
            var bufferedLength = 0;
            var nasha0 = new byte[0];

            for(int i = 0; i < translated.Count; ++i)
            {
                var methodBytes = Settings.Serialize(translated[i]);
                Array.Resize(ref nasha0, nasha0.Length + methodBytes.Count);
                methodBytes.CopyTo(nasha0, bufferedLength);
                Settings.Translated[i].Method.Body.Instructions.Last(x => x.OpCode == OpCodes.Ldc_I4).Operand = bufferedLength;
                bufferedLength += methodBytes.Count;
            }

            mainSection.Add(new ByteArrayChunk(Compress(nasha0)), 1);
            references.Add(new ByteArrayChunk(Compress(Settings.TranslateReference().ToArray())), 1);
            opcodesList.Add(new ByteArrayChunk(NashaSettings.TranslateOpcodes().ToArray()), 1);

            NashaSections.Add(mainSection);
            NashaSections.Add(references);
            NashaSections.Add(opcodesList);
        }

        private static void AddParameters(MethodDef method)
        {
            if (method.Parameters.Count == 0)
            {
                method.Body.Instructions.Add(OpCodes.Ldnull.ToInstruction());
                return;
            }

            method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(method.Parameters.Count));
            method.Body.Instructions.Add(OpCodes.Newarr.ToInstruction(method.Module.CorLibTypes.Object));
            method.Body.Instructions.Add(OpCodes.Dup.ToInstruction());

            for (var i = 0; i < method.Parameters.Count; i++)
            {
                method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(i));
                method.Body.Instructions.Add(OpCodes.Ldarg.ToInstruction(method.Parameters[i]));

                var cor = method.Module.CorLibTypes;
                var param = method.Parameters[i];
                if (!param.IsHiddenThisParameter)
                    if (param.Type != cor.String && param.Type != cor.Object && param.Type != cor.TypedReference)
                    {
                        var spec = new TypeSpecUser(param.Type);
                        method.Body.Instructions.Add(new Instruction(OpCodes.Box, spec));
                    }

                method.Body.Instructions.Add(OpCodes.Stelem_Ref.ToInstruction());
                method.Body.Instructions.Add(OpCodes.Dup.ToInstruction());
            }

            method.Body.Instructions.Remove(method.Body.Instructions.Last());
        }

        private static byte[] Compress(byte[] array)
        {
            using var ms = new MemoryStream();
            using (var def = new DeflateStream(ms, CompressionLevel.Optimal))
            {
                def.Write(array, 0, array.Length);
            }

            return ms.ToArray();
        }
    }
}
