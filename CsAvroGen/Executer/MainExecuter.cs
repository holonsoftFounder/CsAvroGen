using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Executer
{
    public class MainExecuter
    {
        public int Run(ProgramArgs prgArgs)
        {
            var typeInfoData = new TypeInfoData();
            var ti = new TypeInspector();

            new ConfigReader().Read(typeInfoData);


            if (!(string.IsNullOrWhiteSpace(prgArgs.AssemblyName) || string.IsNullOrWhiteSpace(prgArgs.TypeName)))
            {
                try
                {
                    ti.InspectFileBasedCompiledType(prgArgs, typeInfoData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return (int)ReturnCode.TypeInspectionFailed;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(prgArgs.File))
                {
                    try
                    {
                        new TypeCompiler().Compile(prgArgs, typeInfoData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return (int)ReturnCode.TypeCompilationFailed;
                    }

                    try
                    {
                        ti.InspectCompiledType(prgArgs, typeInfoData);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return (int)ReturnCode.TypeInspectionFailed;
                    }
                }
                else
                {
                    Console.WriteLine("Nothing to do - provide an Assembly/Type combination or a valid (compilable) .cs-File with appropriate class(es)");
                    return (int)ReturnCode.NothingToDo;
                }
            }

            try
            {
                new AvroSchemaGenerator().Generate(prgArgs, typeInfoData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Console.WriteLine("Schema generation failed");
                return (int)ReturnCode.SchemaGenerationFailed;
            }

            Console.WriteLine("Done - all is fine!");
            return (int)ReturnCode.Ok;
        }
    }
}
