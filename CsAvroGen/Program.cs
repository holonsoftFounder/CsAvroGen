using System;
using System.Reflection;
using CsAvroGen.Abstractions;
using CsAvroGen.Abstractions.Enums;
using holonsoft.CmdLineParser;
using holonsoft.CsAvroGen.ShowRunner;

namespace holonsoft.CsAvroGen
{
    internal class Program
    {
        static int Main(string[] args)
        {
            var p = new CommandLineParser<ProgramArgs>();
            var prgArgs = p.Parse(args);

            if (prgArgs.Help)
            {
                prgArgs.Version = true;
            }


            if (prgArgs.Version)
            {
                Console.WriteLine("holonsoft csavrogen, Version " + Assembly.GetExecutingAssembly().AssemblyVersion());
            }


            if (prgArgs.Help)
            {
                Console.WriteLine(p.GetConsoleFormattedHelpTexts(Console.WindowWidth));
            }

            if (prgArgs.Help || prgArgs.Version) return (int)ReturnCode.Ok;

            return new MainExecuter().Run(prgArgs);
        }
    }
}
