using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteMapTypeInfo(ExtendedFieldInfo efi)
        {
            switch (efi.AvroType)
            {
                case AvroFieldType.Map:
                    switch (efi.TypeCode)
                    {
                        case TypeCode.Int32:
                            WriteMapType(efi, "int");
                            break;
                        case TypeCode.Single:
                            WriteMapType(efi, "float");
                            break;
                        case TypeCode.Double:
                            WriteMapType(efi, "double");
                            break;
                        case TypeCode.String:
                            WriteMapType(efi, "string");
                            break;
                        default:
                            throw new NotSupportedException("type of " + efi.FieldName + " as map not supported");
                    }
                    break;
                case AvroFieldType.MapWithRecordType:
                    WriteMapType(efi, string.Empty);
                    break;
                default:
                    throw new NotSupportedException("type of " + efi.FieldName + " as map not supported");
            }
        }


        private void WriteMapType(ExtendedFieldInfo efi, string typeName)
        {
            _sb.Append(_indentProvider.Get());
            _sb.Append("{ ");
            _sb.Append("name".ToDoubleQoutedString() + ": ");
            _sb.Append(efi.FieldName.ToDoubleQoutedString());

            if (!WriteDocValue(efi, true))
            {
                _sb.Append(", ");
            }

            _sb.AppendLine("type".ToDoubleQoutedString() + ": {");

            _indentProvider.IncLevel();
            _indentProvider.IncLevel();
            _sb.Append(_indentProvider.Get());
            _sb.Append("type".ToDoubleQoutedString() + ": " + "map".ToDoubleQoutedString() + ", ");
            _sb.Append("values".ToDoubleQoutedString() + ": ");

            
            if (efi.AvroType == AvroFieldType.Map || _generatedTypes.Contains(efi.ImplementingClassName))
            {
                var s = string.IsNullOrWhiteSpace(typeName) ? efi.ImplementingClassName : typeName;

                _sb.Append(s.ToDoubleQoutedString() + " }");
            }
            else
            {
                _indentProvider.DecLevel();
                _indentProvider.DecLevel();
                _sb.AppendLine();

                WriteClassTypeInfo(efi, false);
                _sb.Append(_indentProvider.Get());

                _indentProvider.IncLevel();
                _indentProvider.IncLevel();
            }

            _indentProvider.DecLevel();
            _indentProvider.DecLevel();
            _sb.Append(_indentProvider.Get());
            _sb.AppendLine("}, ");

        }

    }
}