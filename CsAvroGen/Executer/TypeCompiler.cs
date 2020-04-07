using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using CsAvroGen.DomainModel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace holonsoft.CsAvroGen.Executer
{
    public class TypeCompiler
    {
        public void Compile(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            if (!File.Exists(prgArgs.File))
            {
                throw new FileNotFoundException(prgArgs.File);
            }

            var sourceCode = File.ReadAllText(prgArgs.File);

            
            using (var peStream = new MemoryStream())
            {
                var result = GenerateCode(sourceCode, typeInfoData.MetadataReferenceList).Emit(peStream);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (var diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    throw new Exception("Compilation failed");
                }

                Console.WriteLine("Compilation done without any error.");

                peStream.Seek(0, SeekOrigin.Begin);

                typeInfoData.Assembly = Assembly.Load(peStream.ToArray());
            }
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
