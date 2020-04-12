using CsAvroGen.DomainModel.Enums;
using holonsoft.CmdLineParser.DomainModel;

namespace holonsoft.CsAvroGen
{
    public class ProgramArgs
    {
        [Argument(ArgumentTypes.AtMostOnce, ShortName = "fds", HelpText = "Instead of creating a namespace based directory structure just put file in output dir, default is TRUE", DefaultValue = true)]
        public bool FlattenDirStructure = true;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "nlc", HelpText = "Indicate whether namespace should be converted to lowercase (as in Java default), default is TRUE", DefaultValue = true)]
        public bool NamespaceToLowercase = true;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "f", HelpText = "File to be used, class will be compiled 'on fly'")]
        public string File;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "an", HelpText = "Assembly with type to be parsed")]
        public string AssemblyName;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "tn", HelpText = "Type in assembly to be parsed")]
        public string TypeName;

        [Argument(ArgumentTypes.Required, ShortName = "o", HelpText = "Output directory")]
        public string OutDir;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "wch", HelpText = "Write comment header in generated file, default is TRUE", DefaultValue = true)]
        public bool WriteCommentHeaderInAvroFile;

        [Argument(ArgumentTypes.AtMostOnce, ShortName =  "h", OccurrenceSetsBool = true, HelpText = "Show this help and quit")]
        public bool Help;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "v", OccurrenceSetsBool = true, HelpText = "Show version and quit")]
        public bool Version;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "ll", DefaultValue = LogSeverity.Info, HelpText = "Set log level, Verbose|Info|Warn|Error|Fatal")]
        public LogSeverity LogLevel;
    }
}
