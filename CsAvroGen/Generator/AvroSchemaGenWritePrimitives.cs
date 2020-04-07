using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WritePrimitiveTypeInfo(ExtendedFieldInfo extendedFieldInfo, string avroTypeName)
        {
            _sb.Append(_indentProvider.Get());
            _sb.Append("{ ");
            _sb.Append("name".ToDoubleQoutedString() + ": ");
            _sb.Append(extendedFieldInfo.FieldName.ToDoubleQoutedString() + ", ");
            _sb.Append("type".ToDoubleQoutedString() + ": ");

            if (extendedFieldInfo.IsNullable)
            {
                _sb.Append("[" + "null".ToDoubleQoutedString() + ", " + avroTypeName.ToDoubleQoutedString() + "]");
            }
            else
            {
                _sb.Append(avroTypeName.ToDoubleQoutedString());
            }


            WriteDefaultValue(extendedFieldInfo);
            WriteDocValue(extendedFieldInfo);

            _sb.AppendLine(" }, ");


        }
    }
}