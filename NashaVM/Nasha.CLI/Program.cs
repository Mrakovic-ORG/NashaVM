using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Writer;
using Nasha.CLI.Core; 
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression; 
using System.Linq;
using System.Reflection;
using Serilog.Core;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System.Diagnostics;

namespace Nasha.CLI
{
    internal static class Program {

        private static readonly NashaSettings _nashaSettings = new();
        private static readonly List<PESection> _nashaSections = new();
        private static readonly string _nashaVersion = typeof(Program).Assembly.GetName().Version.ToString();
        private static readonly Logger _logger = new LoggerConfiguration().WriteTo.Console(theme: AnsiConsoleTheme.Code).CreateLogger();

        private static void Main(string[] args) {

            Console.Title = $"Nasha v{_nashaVersion} - [NSVM]";
            Console.SetWindowSize(88, 20);
            Console.SetBufferSize(88, 9001);

            if (args.Length <= 0) {
                Console.Write("[~] Module Path: ");
                args = new[] { Console.ReadLine() };
            }

            var timer = Stopwatch.StartNew();
            var module = ModuleDefMD.Load(args[0]);

            var runtimePath = Path.Combine(Path.GetDirectoryName(typeof(Program).Assembly.Location), "Nasha.dll");
            var runtime = ModuleDefMD.Load(runtimePath); 

            IMethod runMethod = ImportRuntimeMethod(runtime, module, "Main", "Execute");
            IMethod runCtor = ImportRuntimeMethod(runtime, module, "Main", ".ctor");
            IMethod configCtor = ImportRuntimeMethod(runtime, module, "Config", ".ctor");
            IMethod configSetup = ImportRuntimeMethod(runtime, module, "Config", "SetupReferences");
            IMethod configDiscover = ImportRuntimeMethod(runtime, module, "Config", "SetupDiscover");
            IMethod configVmBytes = ImportRuntimeMethod(runtime, module, "Config", "SetupBody");

            var configField = new FieldDefUser(GenerateString(), new FieldSig(configCtor.DeclaringType.ToTypeSig()), dnlib.DotNet.FieldAttributes.Public | dnlib.DotNet.FieldAttributes.Static);
            module.GlobalType.Fields.Add(configField);
            var globalConstructor = module.GlobalType.FindOrCreateStaticConstructor();

             
            
            Console.Write("[?] Bypass ObfuscationAttribute: ");
            bool bypass = Console.ReadKey().Key != ConsoleKey.N;
            Console.WriteLine();

            foreach (var type in module.Types.Where(x => !x.IsGlobalModuleType && !x.HasGenericParameters)) { 
                foreach (var method in type.Methods.Where(x => !x.HasGenericParameters)) {

                    if (!bypass && !method.IsNashaMarked())
                        continue;

                    var nashaInstructions = Translator.Translate(_nashaSettings, method);
                    var hasInstruction = nashaInstructions is not null;

                    if (hasInstruction) {
                        _nashaSettings.Translated.Add(new Translated(method, nashaInstructions));
                        _logger.Information("{0} Method Virtualized.", method.Name.String);
                    }
                    else {
                        _logger.Error("Can't Virtualize Method {0}.", method.Name.String); 
                    } 
                }
            }

            // configField = new Config();
            globalConstructor.Body.Instructions.Insert(0, OpCodes.Newobj.ToInstruction(configCtor));
            globalConstructor.Body.Instructions.Insert(1, OpCodes.Stsfld.ToInstruction(configField));

            // configField.SetupReferences()
            globalConstructor.Body.Instructions.Insert(2, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(3, OpCodes.Callvirt.ToInstruction(configSetup));

            // configField.SetupBody()
            globalConstructor.Body.Instructions.Insert(4, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(5, OpCodes.Callvirt.ToInstruction(configVmBytes));

            // configField.SetupDiscover()
            globalConstructor.Body.Instructions.Insert(6, OpCodes.Ldsfld.ToInstruction(configField));
            globalConstructor.Body.Instructions.Insert(7, OpCodes.Callvirt.ToInstruction(configDiscover));

            foreach (var method in _nashaSettings.Translated.Select(x => x.Method)) {

                method.Body = new CilBody() { MaxStack = 1 };
                // Parameters Array.
                method.Body.Variables.Add(new(new SZArraySig(method.Module.CorLibTypes.Object)));
                // Return Object.
                method.Body.Variables.Add(new(method.Module.CorLibTypes.Object));


                var stubExpression = GenerateStub(method, runCtor, configField, runMethod).ToList();

                var parameterExpression = GenerateParametersStub(method);

                // AddParameters(method);

                stubExpression.InsertRange(0, parameterExpression);

                var leaveLabel = new Instruction(OpCodes.Ldloc_1);

                if (method.Parameters.Any(x => x.Type is ByRefSig)) /* (ref, out) param */ {

                    stubExpression.Add(new(OpCodes.Leave, leaveLabel));

                    var refExpression = new List<Instruction>();

                    foreach (var parameter in method.Parameters.Where(x => x.Type is ByRefSig))
                        refExpression.AddRange(ParameterExpression(parameter));

                    refExpression.Add(new(OpCodes.Endfinally));
                    stubExpression.AddRange(refExpression);

                    var exHandler = new ExceptionHandler(ExceptionHandlerType.Finally) {
                        TryStart = stubExpression.First(x => x.OpCode.Code is Code.Newobj),
                        TryEnd = refExpression.First(),
                        HandlerStart = refExpression.First(),
                        HandlerEnd = leaveLabel,
                    };
                    method.Body.ExceptionHandlers.Add(exHandler); 
                }

                foreach (var stubInstruction in stubExpression)
                    method.Body.Instructions.Add(stubInstruction); 

                method.Body.Instructions.Add(leaveLabel);
                method.Body.Instructions.Add(method.HasReturnType
                    ? new(OpCodes.Unbox_Any, method.ReturnType.ToTypeDefOrRef())
                    : new(OpCodes.Pop));
                method.Body.Instructions.Add(new(OpCodes.Ret));

                IEnumerable<Instruction> ParameterExpression(Parameter parameter) {
                    var ret = new List<Instruction>();
                    ret.Add(new(OpCodes.Ldarg, parameter));
                    ret.Add(new(OpCodes.Ldloc_0));
                    ret.Add(new(OpCodes.Ldc_I4, parameter.MethodSigIndex));
                    ret.Add(new(OpCodes.Ldelem_Ref));
                    if (parameter.Type.IsValueType) {
                        ret.Add(new(OpCodes.Unbox_Any, method.Module.Import(parameter.Type)));
                    }
                    // Sorry For Yandere Solution :/
                    if (parameter.Type.Next == module.CorLibTypes.IntPtr)
                        ret.Add(new(OpCodes.Stind_I));
                    else if (parameter.Type.Next == module.CorLibTypes.SByte || parameter.Type.Next == module.CorLibTypes.Byte)
                        ret.Add(new(OpCodes.Stind_I1));
                    else if (parameter.Type.Next == module.CorLibTypes.Int16 || parameter.Type.Next == module.CorLibTypes.UInt16)
                        ret.Add(new(OpCodes.Stind_I2));
                    else if (parameter.Type.Next == module.CorLibTypes.Int32 || parameter.Type.Next == module.CorLibTypes.UInt32)
                        ret.Add(new(OpCodes.Stind_I4));
                    else if (parameter.Type.Next == module.CorLibTypes.Int64 || parameter.Type.Next == module.CorLibTypes.UInt64)
                        ret.Add(new(OpCodes.Stind_I8));
                    else if (parameter.Type.Next == module.CorLibTypes.Single)
                        ret.Add(new(OpCodes.Stind_R4));
                    else if (parameter.Type.Next == module.CorLibTypes.Double)
                        ret.Add(new(OpCodes.Stind_R8));
                    else
                        ret.Add(new(OpCodes.Stind_Ref));
                    return ret;
                }
            }

            var output = args[0].Insert(args[0].Length - 4, "-Nasha");
            var writer = new ModuleWriterOptions(module);

            writer.WriterEvent += InsertSections;
            writer.WriterEvent += InsertVmBody;
            writer.MetadataLogger = DummyLogger.NoThrowInstance;
            writer.MetadataOptions.Flags = MetadataFlags.AlwaysCreateStringsHeap |
                                           MetadataFlags.AlwaysCreateBlobHeap | 
                                           MetadataFlags.AlwaysCreateGuidHeap | 
                                           MetadataFlags.AlwaysCreateUSHeap;
            module.Write(output, writer);

            // Copy Nasha runtime to application path
            var outputDir = Path.GetDirectoryName(args[0]);

            if (Directory.GetCurrentDirectory() != outputDir) {
                File.Copy(runtimePath, outputDir + "\\Nasha.dll", true);
            }

            timer.Stop();
            _logger.Information("Module {0} Writed Successfully.", module.Assembly.Name.String);
            _logger.Information("Finished In {0}.", timer.Elapsed.TotalSeconds);
            Console.ReadKey();
        }

        public static IMethod ImportRuntimeMethod(ModuleDefMD runtimeModule, ModuleDefMD targetModule, string typeName, string methodName) =>
            targetModule.Import(runtimeModule.Types.ToArray().First(x => x.Name == typeName).Methods.First(x => x.Name == methodName));
        public static bool IsNashaMarked(this MethodDef method) {
            if (!method.CustomAttributes
                .Any(x => x.TypeFullName == $"System.Reflection.{nameof(ObfuscationAttribute)}"))
                return false;

            foreach (var ca in method.CustomAttributes.Where(x => x.TypeFullName == $"System.Reflection.{nameof(ObfuscationAttribute)}").ToArray()) {

                if (!ca.Properties.Any(x => x.Name == nameof(ObfuscationAttribute.Feature) && x.Value is string featureName && featureName == "NSVM")) // Native Sharp Virtual Machine.
                    continue;

                if (ca.Properties.Any(x => x.Name == nameof(ObfuscationAttribute.StripAfterObfuscation) && x.Value is bool stripCA && stripCA))
                    method.CustomAttributes.Remove(ca);

                if (ca.Properties.Any(x => x.Name == nameof(ObfuscationAttribute.ApplyToMembers) && x.Value is bool apply && !apply))
                    return false;

                return true;
            }

            return false;
        }

        private static void InsertSections(object sender, ModuleWriterEventArgs e)
        {
            var writer = (ModuleWriterBase)sender;
            if (e.Event == ModuleWriterEvent.MDMemberDefsInitialized)
                _nashaSections.ForEach(x => writer.AddSection(x));
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

            var translated = _nashaSettings.Translated;
            var bufferedLength = 0;
            var nasha0 = new byte[0];

            for (int i = 0; i < translated.Count; ++i)
            {
                var methodBytes = _nashaSettings.Serialize(translated[i]);
                Array.Resize(ref nasha0, nasha0.Length + methodBytes.Count);
                methodBytes.CopyTo(nasha0, bufferedLength);
                _nashaSettings.Translated[i].Method.Body.Instructions.First(x => x.OpCode == OpCodes.Ldc_I4 && (int)x.Operand == 0xFFFFFFF).Operand = bufferedLength;
                bufferedLength += methodBytes.Count;
            }

            mainSection.Add(new ByteArrayChunk(Compress(nasha0)), 1);
            references.Add(new ByteArrayChunk(Compress(_nashaSettings.TranslateReference().ToArray())), 1);
            opcodesList.Add(new ByteArrayChunk(NashaSettings.TranslateOpcodes().ToArray()), 1);

            _nashaSections.Add(mainSection);
            _nashaSections.Add(references);
            _nashaSections.Add(opcodesList);
        }

        private static IEnumerable<Instruction> GenerateStub(
            MethodDef method,
            IMethod mainConstructor,
            IField configField,
            IMethod runMethod) {
            var ret = new List<Instruction>();

            // Initialize New Main Class.
            ret.Add(new(OpCodes.Newobj, mainConstructor));
            // Load Parameters Local.
            ret.Add(new(OpCodes.Ldloc_0));
            // Method Identifier That Will get Patched While Inserting Sections.
            ret.Add(new(OpCodes.Ldc_I4, 0xFFFFFFF));
            // Load Config Field.
            ret.Add(new(OpCodes.Ldsfld, configField));
            // Run VM.
            ret.Add(new(OpCodes.Call, runMethod)); 
            // Store Returned Object.
            ret.Add(new(OpCodes.Stloc_1));


            return ret;
        } 
        private static IEnumerable<Instruction> GenerateParametersStub(MethodDef method) {
            var ret = new List<Instruction>();

            if (method.Parameters.Count is 0) {
                ret.Add(new(OpCodes.Ldnull));
                return ret;
            }

            // Push Parameters Count.
            ret.Add(new(OpCodes.Ldc_I4, method.Parameters.Count));
            // Push new Object Array on The Stack.
            ret.Add(new(OpCodes.Newarr, method.Module.CorLibTypes.Object.ToTypeDefOrRef()));

            for (int x = 0; x < method.Parameters.Count; x++) {
                var parameter = method.Parameters[x];

                var storeParameterExpression = new Instruction[] {
                    // Duplicate Array On The Stack.
                    new(OpCodes.Dup),
                    // Push Item Index.
                    new(OpCodes.Ldc_I4, x),
                    // Load Parameter Value.
                    new(OpCodes.Ldarg, parameter),
                    // Store Parameter Value In Item Index (x).
                    new(OpCodes.Stelem_Ref),
                }.ToList();

                if (parameter.Type is ByRefSig)
                    storeParameterExpression.Insert(3, new(GetLoadRefCode(parameter, method.Module)));

                if (parameter.Type.IsValueType) {
                    storeParameterExpression.Insert(storeParameterExpression.Count is 4 
                        ? 3
                        : 4, new(OpCodes.Box, method.Module.Import(parameter.Type)));
                }

                ret.AddRange(storeParameterExpression);
                 
            }

            // Pop Array From Stack And Store It On Parameters Local.
            ret.Add(new(OpCodes.Stloc_0, method.Body.Variables.First()));

            return ret;
        }  
        private static OpCode GetLoadRefCode(Parameter parameter, ModuleDef module) {
            if (parameter.Type.Next == module.CorLibTypes.IntPtr)
                return OpCodes.Ldind_I;

            else if (parameter.Type.Next == module.CorLibTypes.Byte)
                return OpCodes.Ldind_U1;
            else if (parameter.Type.Next == module.CorLibTypes.SByte)
                return OpCodes.Ldind_I1;

            else if (parameter.Type.Next == module.CorLibTypes.UInt16)
                return OpCodes.Ldind_U2;
            else if (parameter.Type.Next == module.CorLibTypes.Int16)
                return OpCodes.Ldind_I2;

            else if (parameter.Type.Next == module.CorLibTypes.UInt32)
                return OpCodes.Ldind_U4;
            else if (parameter.Type.Next == module.CorLibTypes.Int32)
                return OpCodes.Ldind_I4;

            else if (parameter.Type.Next == module.CorLibTypes.Int64 || parameter.Type.Next == module.CorLibTypes.UInt64)
                return OpCodes.Ldind_I8;

            else if (parameter.Type.Next == module.CorLibTypes.Single)
                return OpCodes.Ldind_R4;
            else if (parameter.Type.Next == module.CorLibTypes.Double)
                return OpCodes.Ldind_R8;

            else
                return OpCodes.Ldind_Ref;
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
        private static string GenerateString(int length = 5) {
            var random = new Random();

            var stringBuffer = new char[length];

            for (int x = 0; x < stringBuffer.Length; x++)
                stringBuffer[x] = (char)random.Next('0', 'z');
            return new string(stringBuffer);
        }
    }
}