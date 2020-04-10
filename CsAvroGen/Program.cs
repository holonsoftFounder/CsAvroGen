using holonsoft.CmdLineParser;
using holonsoft.CsAvroGen.Executer;

namespace holonsoft.CsAvroGen
{
    class Program
    {

        static int Main(string[] args)
        {
            var p = new CommandLineParser<ProgramArgs>();
            var prgArgs = p.Parse(args);

            return new MainExecuter().Run(prgArgs);
        }
    }
}
