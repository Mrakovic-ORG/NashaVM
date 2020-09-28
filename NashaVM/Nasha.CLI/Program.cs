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
    class Program
    {
        private static readonly NashaSettings settings = new NashaSettings();
        private static readonly List<PESection> NashaSections = new List<PESection>();

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Drag'n drop file.");
                Console.ReadKey();
                return;
            }
            var module = ModuleDefMD.Load(args[0]);

            var runtimePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
               "Nasha.dll");
            var runtime = ModuleDefMD.Load(runtimePath);
            IMethod RunMethod = runtime.Types.ToArray().First(x => x.Name == "Main").Methods.First(x => x.Name == "Execute");
            IMethod RunCtor = runtime.Types.ToArray().First(x => x.Name == "Main").Methods.First(x => x.Name == ".ctor");
            IMethod ConfigCtor = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == ".ctor");
            IMethod ConfigSetup = runtime.Types.ToArray().First(x => x.Name == "Config").Methods.First(x => x.Name == "SetupReferencies");

            RunMethod = module.Import(RunMethod);
            RunCtor = module.Import(RunCtor);
            ConfigCtor = module.Import(ConfigCtor);
            ConfigSetup = module.Import(ConfigSetup);

            var ConfigField = new FieldDefUser("cfg", new FieldSig(ConfigCtor.DeclaringType.ToTypeSig()), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            module.GlobalType.Fields.Add(ConfigField);
            var GlobalConstructor = module.GlobalType.FindOrCreateStaticConstructor();
            GlobalConstructor.Body.Instructions.Insert(0, OpCodes.Newobj.ToInstruction(ConfigCtor));
            GlobalConstructor.Body.Instructions.Insert(1, OpCodes.Stsfld.ToInstruction(ConfigField));
            GlobalConstructor.Body.Instructions.Insert(2, OpCodes.Ldsfld.ToInstruction(ConfigField));
            GlobalConstructor.Body.Instructions.Insert(3, OpCodes.Callvirt.ToInstruction(ConfigSetup));

            foreach (var type in module.Types)
            {
                foreach (var method in type.Methods)
                {

                    if (!method.HasBody || !method.Body.HasInstructions) continue;
                    var translated = Translator.Translate(settings, method);
                    if (translated == null)
                        continue;
                    settings.Translated.Add(new Translated(method, translated));
                }
            }
            foreach (var translated in settings.Translated)
            {
                //var body = translated.Method.Body;
                translated.Method.Body = new CilBody() { MaxStack = 1 };
                translated.Method.Body.Instructions.Add(OpCodes.Newobj.ToInstruction(RunCtor));
                AddParameters(translated.Method);
                translated.Method.Body.Instructions.Add(OpCodes.Ldc_I4.ToInstruction(0));

                translated.Method.Body.Instructions.Add(OpCodes.Ldsfld.ToInstruction(ConfigField));
                translated.Method.Body.Instructions.Add(OpCodes.Call.ToInstruction(RunMethod));
                if (translated.Method.HasReturnType)
                    translated.Method.Body.Instructions.Add(OpCodes.Unbox_Any.ToInstruction(translated.Method.ReturnType.ToTypeDefOrRef()));
                else
                    translated.Method.Body.Instructions.Add(OpCodes.Pop.ToInstruction());

                translated.Method.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
            }

            var Output = Path.GetFileNameWithoutExtension(args[0]) + "-Nasha.exe";
            var writer = new ModuleWriterOptions(module);

            writer.WriterEvent += InsertSections;
            writer.WriterEvent += InsertVMBodies;
            writer.MetadataLogger = DummyLogger.NoThrowInstance;
            writer.MetadataOptions.Flags = MetadataFlags.PreserveAll;
            module.Write(Output, writer);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[#] Virtualized file saved as \"{Output}\"");
            Console.ReadKey();
        }

        private static void InsertSections(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;
            if (e.Event == ModuleWriterEvent.MDMemberDefRidsAllocated + 1)
                NashaSections.ForEach(x => writer.AddSection(x));
        }

        private static void InsertVMBodies(object sender, ModuleWriterEventArgs e)
        {
            var MainSection = new PESection(".Nasha0", 0x60000020);
            var Referencies = new PESection(".Nasha1", 0x60000020);
           
            var writer = (ModuleWriterBase)sender;
            TokenGetter.Writer = writer;
            if (e.Event != ModuleWriterEvent.MDMemberDefRidsAllocated)
                return;

            var translateds = settings.Translated;
            var buferedLength = 0;
            var nasha0 = new byte[0];

            for(int i = 0; i < translateds.Count; ++i)
            {
                var methodBytes = settings.Serialize(translateds[i]);
                Array.Resize(ref nasha0, nasha0.Length + methodBytes.Count);
                methodBytes.CopyTo(nasha0, buferedLength);
                settings.Translated[i].Method.Body.Instructions.Last(x => x.OpCode == OpCodes.Ldc_I4).Operand = buferedLength;
                buferedLength += methodBytes.Count;
            }

            MainSection.Add(new ByteArrayChunk(Compress(nasha0)), 1);
            Referencies.Add(new ByteArrayChunk(Compress(settings.TranslateReference().ToArray())), 1);

            NashaSections.Add(MainSection);
            NashaSections.Add(Referencies);
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
            using (var ms = new MemoryStream())
            {
                using (var def = new DeflateStream(ms, CompressionLevel.Optimal))
                {
                    def.Write(array, 0, array.Length);
                }

                return ms.ToArray();
            }
        }
    }
}
