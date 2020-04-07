using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CsAvroGen.DomainModel;
using holonsoft.CsAvroGen.Executer;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly List<string> _generatedTypes = new List<string>();

        private ProgramArgs _prgArgs;

        private IndentProvider _indentProvider;

        internal void Generate(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            _prgArgs = prgArgs;

            _indentProvider = new IndentProvider(" ",  typeInfoData.IndentFactor);
            _indentProvider.Prepare(20);
            _indentProvider.IncLevel();

            if (!_prgArgs.FlattenDirStructure)
            {
                var splitted = Path.Combine(typeInfoData.Namespace.Split('.', StringSplitOptions.RemoveEmptyEntries));
                _prgArgs.OutDir = Path.GetFullPath(Path.Combine(prgArgs.OutDir, splitted));
            }
            else
            {
                _prgArgs.OutDir = Path.GetFullPath(prgArgs.OutDir);
            }

            if (!Directory.Exists(prgArgs.OutDir))
            {
                Directory.CreateDirectory(prgArgs.OutDir);
            }

            _sb.Clear();
            _generatedTypes.Clear();


            _sb.AppendLine("{");

            _sb.Append(CreateCommonHeader());

            if (typeInfoData.DocValue != null)
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("doc".ToDoubleQoutedString());
                _sb.Append(": ");
                _sb.Append(typeInfoData.DocValue.ToDoubleQoutedString());
            }
            _sb.AppendLine(", ");

            _sb.Append(_indentProvider.Get());
            _sb.Append("name".ToDoubleQoutedString());
            _sb.Append(": ");
            _sb.Append(typeInfoData.InspectedType.Name.ToDoubleQoutedString());
            _sb.AppendLine(", ");

            _sb.Append(_indentProvider.Get());
            _sb.Append("type".ToDoubleQoutedString());
            _sb.Append(": ");
            _sb.Append("record".ToDoubleQoutedString());
            _sb.AppendLine(", ");

            _sb.Append(_indentProvider.Get());
            _sb.Append("namespace".ToDoubleQoutedString());
            _sb.Append(": ");
            _sb.Append(prgArgs.NamespaceToLowercase
                            ? typeInfoData.Namespace.ToLowerInvariant().ToDoubleQoutedString()
                            : typeInfoData.Namespace.ToDoubleQoutedString());
            _sb.AppendLine(", ");
            
            WriteRecord(typeInfoData.FieldList);
            
            _sb.AppendLine("}");

            var outFile = Path.Combine(prgArgs.OutDir, _prgArgs.TypeName) + ".avsc";

            File.WriteAllText(outFile, _sb.ToString());
        }

        

        private void WriteRecord(IEnumerable<ExtendedFieldInfo> efi)
        {
            _sb.Append(_indentProvider.Get());
            _sb.AppendLine("fields".ToDoubleQoutedString() + ": [");

            _indentProvider.IncLevel();

            foreach (var extendedFieldInfo in efi)
            {
                WriteFieldToAvro(extendedFieldInfo);
            }

            _indentProvider.DecLevel();

            _sb.AppendLine();
            _sb.Append(_indentProvider.Get());
            _sb.AppendLine("]");

        }
        

        private void WriteDefaultValue(ExtendedFieldInfo extendedFieldInfo)
        {
            if (!extendedFieldInfo.HasDefaultValue) return;

            _sb.Append(", " + "default".ToDoubleQoutedString() + ": " +
                       (extendedFieldInfo.TypeCode == TypeCode.String
                           ? extendedFieldInfo.AvroDefaultValue.ToString().ToDoubleQoutedString()
                           : extendedFieldInfo.AvroDefaultValue));
        }


        private void WriteDocValue(ExtendedFieldInfo extendedFieldInfo)
        {
            if (!extendedFieldInfo.HasDocValue) return;

            _sb.Append(", " + "doc".ToDoubleQoutedString() + ": " + extendedFieldInfo.AvroDocValue);
        }

    }
}