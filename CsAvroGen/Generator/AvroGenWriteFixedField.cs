using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteFixedField(ExtendedFieldInfo efi)
        {
            _sb.Append(_indentProvider.Get());
            _sb.Append("{ ");
            WriteDocValue(efi);
            _sb.Append("name".ToDoubleQoutedString() + ": ");
            _sb.Append(efi.FieldName.ToDoubleQoutedString() + ", ");

            _sb.AppendLine("type".ToDoubleQoutedString() + ": {");

            _indentProvider.IncLevel();
            _indentProvider.IncLevel();
            _sb.Append(_indentProvider.Get());
            _sb.Append("name".ToDoubleQoutedString() + ": " + efi.FixedFieldClassName.ToDoubleQoutedString() + ", ");
            _sb.Append("type".ToDoubleQoutedString() + ": " + "fixed".ToDoubleQoutedString() + ", ");
            _sb.Append("size".ToDoubleQoutedString() + ": ");
            _sb.Append(efi.FixedFieldSize);

            WriteDocValue(efi);

            _sb.Append("} ");

            _sb.AppendLine("}, ");
        }
    }
}