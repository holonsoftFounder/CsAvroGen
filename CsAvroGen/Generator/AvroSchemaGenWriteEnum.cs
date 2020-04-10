using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteEnumTypeInfo(ExtendedFieldInfo extendedFieldInfo)
        {
            if (_generatedTypes.Contains(extendedFieldInfo.FieldInfo.FieldType.Name))
            {
                _sb.Append(_indentProvider.Get());
                _sb.Append("{ ");
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.Append(extendedFieldInfo.FieldName.ToDoubleQoutedString() + ", ");
                _sb.Append("type".ToDoubleQoutedString() + ":  ");
                _sb.Append(extendedFieldInfo.FieldInfo.FieldType.Name.ToDoubleQoutedString());
                _sb.AppendLine(" }, ");
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
                _sb.AppendLine("enum".ToDoubleQoutedString() + ", ");
                _sb.Append(_indentProvider.Get());
                _sb.Append("name".ToDoubleQoutedString() + ": ");
                _sb.AppendLine(extendedFieldInfo.FieldInfo.FieldType.Name.ToDoubleQoutedString() + ", ");
                _sb.Append(_indentProvider.Get());
                _sb.Append("symbols".ToDoubleQoutedString() + ": [");

                var comma = false;
                foreach (var s in Enum.GetNames(extendedFieldInfo.FieldInfo.FieldType))
                {
                    if (comma) _sb.Append(", ");

                    _sb.Append(s.ToDoubleQoutedString());
                    comma = true;
                }

                _sb.AppendLine("]");

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("} ");

                _sb.Append(_indentProvider.Get());
                _sb.AppendLine("}, ");

                _generatedTypes.Add(extendedFieldInfo.FieldInfo.FieldType.Name);
            }
            finally
            {
                _indentProvider.DecLevel();
                _indentProvider.DecLevel();
            }
        }
    }
}