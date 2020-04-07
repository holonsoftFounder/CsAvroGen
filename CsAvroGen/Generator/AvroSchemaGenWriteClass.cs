using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteClassTypeInfo(ExtendedFieldInfo extendedFieldInfo)
        {
            if (_generatedTypes.Contains(extendedFieldInfo.ImplementingClassName))
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("{ ");
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.Append(extendedFieldInfo.FieldName.ToDoubleQoutedString() + ", ");
                _sb.Append("type".ToDoubleQoutedString() + ":  ");
                _sb.Append(extendedFieldInfo.ImplementingClassName.ToDoubleQoutedString());
                _sb.AppendLine(" }");

                return;
            }

            try
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("{ ");
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.AppendLine(extendedFieldInfo.FieldName.ToDoubleQoutedString() + ", ");
                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("  " + "type".ToDoubleQoutedString() + ": { ");

                _indentProvider.IncLevel();
                _indentProvider.IncLevel();

                _sb.Append(_indentProvider.Get());
                _sb.Append("type".ToDoubleQoutedString() + ": ");
                _sb.AppendLine("record".ToDoubleQoutedString() + ", ");
                _sb.Append(_indentProvider.Get());
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.AppendLine(extendedFieldInfo.ImplementingClassName.ToDoubleQoutedString() + ", ");

                WriteRecord(extendedFieldInfo.SubFieldList);

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("} ");

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("}, ");

                _generatedTypes.Add(extendedFieldInfo.ImplementingClassName);
            }
            finally
            {
                _indentProvider.DecLevel();
                _indentProvider.DecLevel();
            }


        }
    }
}