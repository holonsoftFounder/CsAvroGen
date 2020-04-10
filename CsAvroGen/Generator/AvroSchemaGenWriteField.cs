using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteFieldToAvro(ExtendedFieldInfo efi)
        {
            _indentProvider.IncLevel();

            try
            {
                switch (efi.AvroType)
                {
                    case AvroFieldType.Array:
                    case AvroFieldType.ArrayWithRecordType:
                        WriteArrayTypeInfo(efi);
                        break;
                    case AvroFieldType.Map:
                    case AvroFieldType.MapWithRecordType:
                        WriteMapTypeInfo(efi);
                        break;
                    case AvroFieldType.Record:
                        WriteClassTypeInfo(efi);
                        break;
                    case AvroFieldType.Enum:
                        WriteEnumTypeInfo(efi);
                        break;
                    case AvroFieldType.Boolean:
                        WritePrimitiveTypeInfo(efi, "boolean");
                        return;
                    case AvroFieldType.Int:
                        WritePrimitiveTypeInfo(efi, "int");
                        break;
                    case AvroFieldType.Long:
                        WritePrimitiveTypeInfo(efi, "long");
                        return;
                    case AvroFieldType.Float:
                        WritePrimitiveTypeInfo(efi, "float");
                        return;
                    case AvroFieldType.Double:
                        WritePrimitiveTypeInfo(efi, "double");
                        return;
                    case AvroFieldType.String:
                        WritePrimitiveTypeInfo(efi, "string");
                        return;
                    case AvroFieldType.Fixed:
                        WriteFixedField(efi);
                        break;
                    default:
                        throw new NotSupportedException("type of " + efi.FieldName + " not supported");
                }
            }
            finally
            {
                _indentProvider.DecLevel();
            }
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

            _sb.Append(", " + "doc".ToDoubleQoutedString() + ": " + extendedFieldInfo.AvroDocValue.ToDoubleQoutedString());
        }
    }
}