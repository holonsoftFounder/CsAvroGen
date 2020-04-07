using System;
using CsAvroGen.DomainModel;

namespace holonsoft.CsAvroGen.Generator
{
    internal partial class AvroSchemaGenerator
    {
        private void WriteFieldToAvro(ExtendedFieldInfo extendedFieldInfo)
        {
            _indentProvider.IncLevel();

            try
            {
                // https://avro.apache.org/docs/current/spec.html#schema_primitive


                if (extendedFieldInfo.IsArray)
                {
                    //WriteArrayTypeInfo(indentStr, extendedFieldInfo, "");
                    return;
                }

                if (extendedFieldInfo.IsMap)
                {
                    //WriteMapTypeInfo(indentStr, extendedFieldInfo, "");
                    return;
                }

                if (extendedFieldInfo.IsClass)
                {
                    WriteClassTypeInfo(extendedFieldInfo);
                    return;
                }


                switch (extendedFieldInfo.TypeCode)
                {
                    // primitives
                    case TypeCode.Empty:
                        break;
                    case TypeCode.Boolean:
                        WritePrimitiveTypeInfo(extendedFieldInfo, "boolean");
                        return;
                    case TypeCode.Int64:
                        WritePrimitiveTypeInfo(extendedFieldInfo, "long");
                        return;
                    case TypeCode.Single:
                        WritePrimitiveTypeInfo(extendedFieldInfo, "float");
                        return;
                    case TypeCode.Double:
                        WritePrimitiveTypeInfo(extendedFieldInfo, "double");
                        return;
                    case TypeCode.String:
                        WritePrimitiveTypeInfo(extendedFieldInfo, "string");
                        return;

                    // primitives or complex if enum
                    case TypeCode.Int32:
                        if (extendedFieldInfo.FieldInfo.FieldType.BaseType == typeof(Enum))
                        {
                            WriteEnumTypeInfo(extendedFieldInfo);
                        }
                        else
                        {
                            WritePrimitiveTypeInfo(extendedFieldInfo, "int");
                        }
                        return;

                    // logical type
                    case TypeCode.Decimal:

                        return;

                }
            }
            finally
            {
                _indentProvider.DecLevel();
            }

        }
    }
}