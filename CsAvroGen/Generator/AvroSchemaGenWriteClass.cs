using CsAvroGen.Abstractions;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteClassTypeInfo(ExtendedFieldInfo efi, bool needTypeString = true)
        {
            if (_generatedTypes.Contains(efi.ImplementingClassName))
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("{ ");
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.Append(efi.FieldName.ToDoubleQoutedString() + ", ");
                _sb.Append("type".ToDoubleQoutedString() + ":  ");
                _sb.Append(efi.ImplementingClassName.ToDoubleQoutedString());
                _sb.AppendLine(" }");

                return;
            }

            try
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("{ ");

                if (needTypeString)
                {
                    _sb.Append("name".ToDoubleQoutedString() + ": ");
                    _sb.AppendLine(efi.FieldName.ToDoubleQoutedString() + ", ");
                }
                _sb.Append(_indentProvider.Get());

                if (needTypeString)
                {
                    _sb.AppendLine("  " + "type".ToDoubleQoutedString() + ": { ");
                }

                _indentProvider.IncLevel();
                _indentProvider.IncLevel();

                _sb.Append(_indentProvider.Get());
                _sb.Append("type".ToDoubleQoutedString() + ": ");
                _sb.AppendLine("record".ToDoubleQoutedString() + ", ");
                _sb.Append(_indentProvider.Get());
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.AppendLine(efi.ImplementingClassName.ToDoubleQoutedString() + ", ");

                WriteRecord(efi.SubFieldList);

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("} ");

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("}, ");

                _generatedTypes.Add(efi.ImplementingClassName);
            }
            finally
            {
                _indentProvider.DecLevel();
                _indentProvider.DecLevel();
            }


        }
    }
}