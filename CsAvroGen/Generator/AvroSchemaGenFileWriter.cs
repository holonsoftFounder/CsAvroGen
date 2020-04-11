using System;
using System.IO;
using System.Text;
using holonsoft.CsAvroGen.Executer;
using Microsoft.CodeAnalysis.Emit;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteAvroFile()
        {
            var outFile = Path.Combine(_prgArgs.OutDir, _prgArgs.TypeName) + ".avsc";

            //var prettyOutputStr = MakePrettyOutput();

            File.WriteAllText(outFile, _sb.ToString());
        }


        private string MakePrettyOutput()
        {
            var s = _sb.Replace(Environment.NewLine, string.Empty).ToString();

            var result = new StringBuilder();

            var lastChar = (char) 0;
            foreach (char inspectedChar in s)
            {
                if (lastChar == ' ' && inspectedChar == lastChar) continue;

                result.Append(inspectedChar);

                switch (inspectedChar)
                {
                    case '{':
                    case '[':
                        _indentProvider.IncLevel();
                        result.AppendLine();
                        result.Append(_indentProvider.Get());
                        break;
                    case '}':
                    case ']':
                        _indentProvider.DecLevel();
                        result.AppendLine();
                        result.Append(_indentProvider.Get());
                        break;
                }

                lastChar = inspectedChar;
            }

            return result.ToString();
        }
    }
}