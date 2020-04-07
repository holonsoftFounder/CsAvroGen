using holonsoft.CmdLineParser.DomainModel;

namespace holonsoft.CsAvroGen
{
    public class ProgramArgs
    {
        [Argument(ArgumentTypes.AtMostOnce, ShortName = "fds", HelpText = "Instead of creating a namespace based directory structure just put file in output dir", DefaultValue = true)]
        public bool FlattenDirStructure = true;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "nlc", HelpText = "Indicate whether namespace should be converted to lowercase", DefaultValue = true)]
        public bool NamespaceToLowercase = true;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "f", HelpText = "File to be used, class will be compiled 'on fly'")]
        public string File;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "an", HelpText = "Assembly with type to be parsed")]
        public string AssemblyName;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "tn", HelpText = "Type in assembly to be parsed")]
        public string TypeName;

        [Argument(ArgumentTypes.Required, ShortName = "o", HelpText = "Output directory")]
        public string OutDir;

        [Argument(ArgumentTypes.AtMostOnce, ShortName = "wch", HelpText = "Write comment header in generated file", DefaultValue = true)]
        public bool WriteCommentHeaderInAvroFile;
    }
}
