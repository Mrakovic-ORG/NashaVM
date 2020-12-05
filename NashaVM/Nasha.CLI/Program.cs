using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using Nasha.CLI.Core;
using System.Drawing;
using Console = Colorful.Console;
using Extensions = Nasha.CLI.Core.Extensions;
using Nasha.CLI.Helpers;

namespace Nasha.CLI
{
    internal static class Program
    {
        private static readonly NashaSettings Settings = new NashaSettings();
        private static readonly List<PESection> NashaSections = new List<PESection>();

        private static readonly Color SuccessDarkColor = ColorTranslator.FromHtml("#283593");
        private static readonly Color SuccessLightColor = ColorTranslator.FromHtml("#3F51B5");
        private static readonly Color FailedDarkColor = ColorTranslator.FromHtml("#6A1B9A");
        private static readonly Color FailedLightColor = ColorTranslator.FromHtml("#9C27B0");

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Extensions.WriteLineFormatted("Please {0} your file.", FailedLightColor, "drag and drop".ToColor(FailedDarkColor));
                Console.ReadKey();
                return;
            }
            var module = ModuleDefMD.Load(args[0]);

            var runtimePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Nasha.dll");
            var runtime = ModuleDefMD.Load(runtimePath);

            IMethod runMethod = Injection.InjectRuntimeMethod(runtime, module, "Main", "Execute");
            IMethod runCtor = Injection.InjectRuntimeMethod(runtime, module, "Main", ".ctor");
            IMethod configCtor = Injection.InjectRuntimeMethod(runtime, module, "Config", ".ctor");
            IMethod configSetup = Injection.InjectRuntimeMethod(runtime, module, "Config", "SetupReferences");
            IMethod configDiscover = Injection.InjectRuntimeMethod(runtime, module, "Config", "SetupDiscover");
            IMethod configVmBytes = Injection.InjectRuntimeMethod(runtime, module, "Config", "SetupBody");

            var configField = new FieldDefUser("cfg", new FieldSig(configCtor.DeclaringType.ToTypeSig()), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            module.GlobalType.Fields.Add(configField);
            var globalConstructor = module.GlobalType.FindOrCreateStaticConstructor();

            Console.WriteLine("Do you want bypass ObfuscationAttributes? y/n (default y) ");
            bool bypass = Console.ReadKey().Key != ConsoleKey.N;

            foreach (var type in module.Types)
            {
                if (type.IsGlobalModuleType) continue;
                foreach (var method in type.Methods)
                {
                    if (bypass)
                        goto virt;
                    foreach (var attr in method.CustomAttributes)
                        if (attr.AttributeType.TypeName == method.Module.Import(typeof(System.Reflection.ObfuscationAttribute)).TypeName)
                        {
                            if (attr.Properties.FirstOrDefault(x => x.Name == "Feature" && x.Value.ToString() == "virt") != null)
                            {

                                // If StripAfterObfuscation is set to true Nasha will remove the attribute from the output assembly.
                                if (attr.Properties.FirstOrDefault(x => x.Name == "StripAfterObfuscation") is var stripAfterObfuscation && stripAfterObfuscation != null)
                                {
                                    var stripObf = (bool)stripAfterObfuscation.Value;
                                    Extensions.WriteLineFormatted("{0}\n{1}: {2}\n\n", "StripAfterObfuscation".ToColor(stripObf ? SuccessDarkColor : FailedDarkColor), "Method".ToColor(stripObf ? SuccessDarkColor : FailedDarkColor), method.Name.ToString().ToColor(stripObf ? SuccessLightColor : FailedLightColor));

                                    if (stripObf) method.CustomAttributes.Remove(attr);
                                }
                                else
                                {
                                    Extensions.WriteLineFormatted("{0}\n{1}: {2}\n\n", "StripAfterObfuscation".ToColor(SuccessDarkColor), "Method".ToColor(SuccessDarkColor), method.Name.ToString().ToColor(SuccessLightColor));

                                    method.CustomAttributes.Remove(attr);
                                }

                                // If ApplyToMembers is set to true Nasha will apply the virtualization to the method itself.
                                if (attr.Properties.FirstOrDefault(x => x.Name == "ApplyToMembers") is var applyToMembers && applyToMembers != null)
                                {
                                    var applyMembers = (bool)applyToMembers.Value;
                                    Extensions.WriteLineFormatted("{0}\n{1}: {2}\n\n", "ApplyToMembers".ToColor(applyMembers ? SuccessDarkColor : FailedDarkColor), "Method".ToColor(applyMembers ? SuccessDarkColor : FailedDarkColor), method.Name.ToString().ToColor(applyMembers ? SuccessLightColor : FailedLightColor));

                                    if (!applyMembers) continue;
                                }

                                goto virt;
                            }
                        }

                    continue;

                virt:
                    var nashaInstructions = Translator.Translate(Settings, method) ?? null;
                    var hasInstruction = nashaInstructions != null;
                    if (hasInstruction) Settings.Translated.Add(new Translated(method, nashaInstructions));

                    if (hasInstruction)
                    {
                        Extensions.WriteLineFormatted("{0}\n{1}: {2}\n{3}: {4}\n\n", "Virtualized".ToColor(SuccessDarkColor), $"Method".ToColor(SuccessDarkColor), method.Name.ToString().ToColor(SuccessLightColor), "Instructions".ToColor(SuccessDarkColor), nashaInstructions.Count.ToString().ToColor(SuccessLightColor));
                    }
                    else
                    {
                        Extensions.WriteLineFormatted("{0}\n{1}: {2}\n\n", "Failed Virtualizing".ToColor(FailedDarkColor), $"Method".ToColor(FailedDarkColor), method.Name.ToString().ToColor(FailedLightColor));
                    }

                }
            }

            globalConstructor.Body.Instructions.Insert(0, OpCodes.Newobj.ToInstruction(configCtor));
            globalConstructor.Body.Instructions.Insert(1, OpCodes.Stsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(2, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(3, OpCodes.Callvirt.ToInstruction(configSetup));
            globalConstructor.Body.Instructions.Insert(4, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(5, OpCodes.Callvirt.ToInstruction(configDiscover));
            globalConstructor.Body.Instructions.Insert(4, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(5, OpCodes.Callvirt.ToInstruction(configVmBytes));

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

            var output = Path.GetFileNameWithoutExtension(args[0]) + ".Nasha.exe";
            var writer = new ModuleWriterOptions(module);

            writer.WriterEvent += InsertSections;
            writer.WriterEvent += InsertVmBody;
            writer.MetadataLogger = DummyLogger.NoThrowInstance;
            writer.MetadataOptions.Flags = MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateBlobHeap | MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateUSHeap;
            module.Write(output, writer);

            // Copy Nasha runtime to application path
            var outputDir = Path.GetDirectoryName(args[0]);
            if (Directory.GetCurrentDirectory() != outputDir) File.Copy(runtimePath, outputDir + "\\Nasha.dll", true);


            Extensions.WriteLineFormatted("Virtualized file saved in \"{0}\"\n\n", $"{outputDir}\\{output}".ToColor(SuccessLightColor));
            Console.WriteWithGradient(@"M""""""""""""""`YM                   dP                M""""MMMMM""""M M""""""""""`'""""""`YM
M  mmmm.  M                   88                M  MMMMM  M M  mm.  mm.  M
M  MMMMM  M .d8888b. .d8888b. 88d888b. .d8888b. M  MMMMP  M M  MMM  MMM  M
M  MMMMM  M 88'  `88 Y8ooooo. 88'  `88 88'  `88 M  MMMM'. M M  MMM  MMM  M
M  MMMMM  M 88.  .88       88 88    88 88.  .88 M  MMP' .MM M  MMM  MMM  M 
M  MMMMM  M `88888P8 `88888P' dP    dP `88888P8 M     .dMMM M  MMM  MMM  M 
MMMNASHAMMM                                     MMMMMMMMMMM MMMMMMMMMMMMMM", SuccessLightColor, FailedLightColor, 6);
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

            for (int i = 0; i < translated.Count; ++i)
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
