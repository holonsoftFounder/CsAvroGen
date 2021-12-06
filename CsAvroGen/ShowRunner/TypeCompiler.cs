using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsAvroGen.Abstractions;
using CsAvroGen.Abstractions.AvroAttributes;
using CsAvroGen.Abstractions.Enums;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace holonsoft.CsAvroGen.ShowRunner
{
    public class TypeCompiler
    {
        private readonly List<string> _usingList = new List<string>();
        private readonly List<string> _fileNameList = new List<string>();

        private ILogging _logger;

        public void Compile(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            _logger = typeInfoData.Logger;

            _usingList.Clear();
            _fileNameList.Clear();

            var sourceCode = LoadSourceCode(prgArgs.File);

            var u = _usingList.Distinct().Aggregate((i, j) => i + Environment.NewLine + j);

            sourceCode = u + Environment.NewLine + sourceCode;


            using (var peStream = new MemoryStream())
            {
                var result = GenerateCode(sourceCode, typeInfoData.MetadataReferenceList).Emit(peStream);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        _logger.LogIt(LogSeverity.Error,"{0}: {1} at {2}", diagnostic.Id, diagnostic.GetMessage(), diagnostic.Location);
                    }

                    _logger.LogIt(LogSeverity.Fatal, "i18n::Compilation failed");
                    throw new Exception("Compilation failed");
                }

                _logger.LogIt(LogSeverity.Info, "i18n::Compilation done without any error.");

                peStream.Seek(0, SeekOrigin.Begin);
                typeInfoData.Assembly = Assembly.Load(peStream.ToArray());
            }
        }


        private string  LoadSourceCode(string sourceCodeFile)
        {
            sourceCodeFile = Path.GetFullPath(sourceCodeFile);

            if (!File.Exists(sourceCodeFile))
            {
                throw new FileNotFoundException(sourceCodeFile);
            }

            if (_fileNameList.Contains(sourceCodeFile))
            {
                _logger.LogIt(LogSeverity.Warn, "i18n::Warning: recursion detected, {0} already loaded", sourceCodeFile);
                return string.Empty;
            }


            _fileNameList.Add(sourceCodeFile);

            var sb = new StringBuilder();

            var listOfAdditionalFiles = new List<string>();

            foreach (var sl in File.ReadLines(sourceCodeFile))
            {
                if (sl.Contains("using ", StringComparison.InvariantCulture))
                {
                    _usingList.Add(sl);
                    continue;
                }

                if (sl.Contains("//@meta::include", StringComparison.InvariantCultureIgnoreCase))
                {
                    var fileName = sl.Replace("//@meta::include", string.Empty, StringComparison.InvariantCultureIgnoreCase)
                                            .Replace("\"", string.Empty, StringComparison.CurrentCultureIgnoreCase);

                    fileName = Path.GetFileName(Path.GetFullPath(fileName));

                    listOfAdditionalFiles.Add(Path.Combine(Path.GetDirectoryName(sourceCodeFile) ,fileName));
                    continue;
                }

                sb.AppendLine(sl);
            }


            foreach (var fileName in listOfAdditionalFiles)
            {
                sb.AppendLine(LoadSourceCode(fileName));
            }

            return sb.ToString();
        }


        // Thanks to
        // https://laurentkempe.com/2019/02/18/dynamically-compile-and-run-code-using-dotNET-Core-3.0/
        // https://josephwoodward.co.uk/2016/12/in-memory-c-sharp-compilation-using-roslyn

        private CSharpCompilation GenerateCode(string sourceCode, List<MetadataReference> metadataReferenceList)
        {
            var codeString = SourceText.From(sourceCode);
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Default);

            var parsedSyntaxTree = SyntaxFactory.ParseSyntaxTree(codeString, options);

            var references = new List<MetadataReference>
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(AvroDefaultValueAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Attribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Array).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location),
            };

            references.AddRange(metadataReferenceList);

            return CSharpCompilation.Create("TypeInspectionOnly.dll",
                new[] { parsedSyntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }

    }
}
