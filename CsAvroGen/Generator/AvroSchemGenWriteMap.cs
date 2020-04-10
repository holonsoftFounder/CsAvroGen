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
                            WriteMapPrimitiveType(efi, "int");
                            break;
                        case TypeCode.Single:
                            WriteMapPrimitiveType(efi, "float");
                            break;
                        case TypeCode.Double:
                            WriteMapPrimitiveType(efi, "double");
                            break;
                        case TypeCode.String:
                            WriteMapPrimitiveType(efi, "string");
                            break;
                        default:
                            throw new NotSupportedException("type of " + efi.FieldName + " as map not supported");
                    }
                    break;
                case AvroFieldType.MapWithRecordType:
                    //WriteMapComplexType(efi);
                    break;
                default:
                    throw new NotSupportedException("type of " + efi.FieldName + " as map not supported");
            }
        }


        private void WriteMapPrimitiveType(ExtendedFieldInfo extendedFieldInfo, string typeName)
        {
            _sb.Append(_indentProvider.Get());
            _sb.Append("{ ");
            WriteDocValue(extendedFieldInfo);
            _sb.Append("name".ToDoubleQoutedString() + ": ");
            _sb.Append(extendedFieldInfo.FieldName.ToDoubleQoutedString() + ", ");

            _sb.AppendLine("type".ToDoubleQoutedString() + ": {");

            _indentProvider.IncLevel();
            _indentProvider.IncLevel();
            _sb.Append(_indentProvider.Get());
            _sb.Append("type".ToDoubleQoutedString() + ": " + "map".ToDoubleQoutedString() + ", ");
            _sb.Append("values".ToDoubleQoutedString() + ": ");
            _sb.Append(typeName.ToDoubleQoutedString() + " }");
            _indentProvider.DecLevel();
            _indentProvider.DecLevel();
            _sb.Append("}, ");

            _sb.AppendLine();
        }


        private void WriteMapComplexType(ExtendedFieldInfo efi)
        {

        }

    }
}