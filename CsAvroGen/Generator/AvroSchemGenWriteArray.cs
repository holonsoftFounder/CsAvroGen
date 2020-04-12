using System;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.Enums;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteArrayTypeInfo(ExtendedFieldInfo efi)
        {
            switch (efi.AvroType)
            {
                case AvroFieldType.Array:
                    switch (efi.TypeCode)
                    {
                        case TypeCode.Int32:
                            WriteArrayType(efi, "int");
                            break;
                        case TypeCode.Single:
                            WriteArrayType(efi, "float");
                            break;
                        case TypeCode.Double:
                            WriteArrayType(efi, "double");
                            break;
                        case TypeCode.String:
                            WriteArrayType(efi, "string");
                            break;
                        default:
                            throw new NotSupportedException("type of " + efi.FieldName + " as array not supported");
                    }
                    break;
                case AvroFieldType.ArrayWithRecordType:
                    WriteArrayType(efi, string.Empty);
                    break;
                default:
                    throw new NotSupportedException("type of " + efi.FieldName + " as array not supported");
            }
        }


        private void WriteArrayType(ExtendedFieldInfo efi, string typeName)
        {
            _sb.Append(_indentProvider.Get());
            _sb.Append("{ ");
            _sb.Append("name".ToDoubleQoutedString() + ": ");
            _sb.Append(efi.FieldName.ToDoubleQoutedString());
            if (!WriteDocValue(efi, true))
            {
                _sb.Append(", " );
            }

            _sb.AppendLine("type".ToDoubleQoutedString() + ": {");
            
            _indentProvider.IncLevel();
            _indentProvider.IncLevel();
            _sb.Append(_indentProvider.Get());
            _sb.Append("type".ToDoubleQoutedString() + ": " + "array".ToDoubleQoutedString() + ", ");
            _sb.Append("items".ToDoubleQoutedString() + ": ");

            if (efi.AvroType == AvroFieldType.Array || _generatedTypes.Contains(efi.ImplementingClassName))
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
                //_sb.AppendLine("}, ");

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