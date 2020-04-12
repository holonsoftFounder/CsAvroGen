using System;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.Enums;
using holonsoft.CsAvroGen.Generator;

namespace holonsoft.CsAvroGen.ShowRunner
{
    public class MainExecuter
    {
        public int Run(ProgramArgs prgArgs)
        {
            var logger = new I18NLogger(prgArgs);

            var typeInfoData = new TypeInfoData(logger);

            new ConfigReader().Read(typeInfoData);

            var ti = new TypeInspector();

            if (!(string.IsNullOrWhiteSpace(prgArgs.AssemblyName) || string.IsNullOrWhiteSpace(prgArgs.TypeName)))
            {
                try
                {
                    ti.InspectFileBasedCompiledType(prgArgs, typeInfoData);
                }
                catch (Exception ex)
                {
                    logger.LogIt(LogSeverity.Fatal, ex.Message);
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
                        logger.LogIt(LogSeverity.Fatal, ex.Message);
                        return (int)ReturnCode.TypeCompilationFailed;
                    }

                    try
                    {
                        ti.InspectCompiledType(prgArgs, typeInfoData);
                    }
                    catch (Exception ex)
                    {
                        logger.LogIt(LogSeverity.Fatal, ex.Message);
                        return (int)ReturnCode.TypeInspectionFailed;
                    }
                }
                else
                {
                    logger.LogIt(LogSeverity.Info, "i18n::NothingToDo");
                    return (int)ReturnCode.NothingToDo;
                }
            }

            try
            {
                new AvroSchemaGenerator().Generate(prgArgs, typeInfoData);
            }
            catch (Exception ex)
            {
                logger.LogIt(LogSeverity.Fatal, ex.Message);
                logger.LogIt(LogSeverity.Fatal, "i18n::Schema generation failed" + Environment.NewLine);

                return (int)ReturnCode.SchemaGenerationFailed;
            }

            logger.LogIt(LogSeverity.Info, "i18n::Done - all is fine!");
            return (int)ReturnCode.Ok;
        }
    }
}
