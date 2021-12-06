﻿using System;
using System.Reflection;
using CsAvroGen.Abstractions;
using CsAvroGen.Abstractions.AvroAttributes;
using CsAvroGen.Abstractions.Enums;

namespace holonsoft.CsAvroGen.ShowRunner
{
    internal class FieldInspector
    {
        public void Inspect(ILogging logger, ExtendedFieldInfo efi)
        {
            var fieldType = efi.FieldInfo.FieldType;
            efi.TypeCode = Type.GetTypeCode(fieldType);


            SetTypeCodeForPrimitives(efi);

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var t = Nullable.GetUnderlyingType(fieldType);

                if (t != null)
                {
                    efi.TypeCode = Type.GetTypeCode(t);
                    efi.IsNullable = true;

                    SetTypeCodeForPrimitives(efi);
                }
            }


            if (fieldType.IsArray)
            {
                efi.TypeCode = Type.GetTypeCode(fieldType.GetElementType());
                efi.AvroType = AvroFieldType.Array;
            }


            if (fieldType.IsClass)
            {
                efi.ImplementingClassName = fieldType.Name;
            }

            if (efi.IsClass && efi.TypeCode != TypeCode.String && !fieldType.IsArray)
            {
                efi.AvroType = AvroFieldType.Record;
            }



            if (fieldType.IsGenericType)
            {
                var underlyingName = fieldType.UnderlyingSystemType.Name;

                efi.IsMap = underlyingName.Contains("Dictionary", StringComparison.InvariantCultureIgnoreCase)
                            || underlyingName.Contains("SortedList", StringComparison.InvariantCultureIgnoreCase)
                            || underlyingName.Contains("SortedDictionary", StringComparison.InvariantCultureIgnoreCase);

                if (efi.IsMap)
                {
                    if (fieldType.GenericTypeArguments[0] != typeof(string))
                    {
                        logger.LogIt(LogSeverity.Fatal, "i18n::Key of AVRO::MAP must be a string, error in field " + efi.FieldName);
                        throw new NotSupportedException();
                    }

                    efi.TypeCode = Type.GetTypeCode(fieldType.GenericTypeArguments[1]);

                    efi.AvroType = AvroFieldType.Map;


                    if (efi.TypeCode == TypeCode.Object)
                    {
                        efi.AvroType = AvroFieldType.MapWithRecordType;

                        efi.ComplexArrayOrMapType = fieldType.GenericTypeArguments[1];
                        efi.ImplementingClassName = efi.ComplexArrayOrMapType.Name;
                    }

                }
            }


            if (fieldType.IsClass && fieldType.IsArray)
            {
                efi.ImplementingClassName = fieldType.Name.Replace("[]", string.Empty, StringComparison.InvariantCultureIgnoreCase);

                if (efi.TypeCode == TypeCode.Object)
                {
                    efi.AvroType = AvroFieldType.ArrayWithRecordType;

                    efi.ComplexArrayOrMapType = fieldType.GetElementType();
                }
            }


            efi.AvroDefaultValue = efi.FieldInfo.GetCustomAttribute<AvroDefaultValueAttribute>()?.DefaultValue;
            efi.HasDefaultValue = efi.AvroDefaultValue != null;


            var fsAttr = efi.FieldInfo.GetCustomAttribute<AvroFixedAttribute>();
            if (fsAttr != null)
            {
                efi.AvroType = AvroFieldType.Fixed;
                efi.FixedFieldSize = fsAttr.Size;
                efi.FixedFieldClassName = fsAttr.DataClassName;

                if (efi.TypeCode != TypeCode.Byte)
                {
                    logger.LogIt(LogSeverity.Fatal, "i18n::FIXED is not allowed for other types than type BYTE, field: " + efi.FieldName);
                    throw new NotSupportedException("FIXED is not allowed for other types than type BYTE, field: " + efi.FieldName);
                }

                if (string.IsNullOrWhiteSpace(efi.FixedFieldClassName))
                {
                    logger.LogIt(LogSeverity.Fatal, "i18n::You must provide a DataClassName via attribute, field: " + efi.FieldName);
                    throw new ArgumentException("You must provide a DataClassName via attribute, field: " + efi.FieldName);
                }
            }

            var aliasAttr = efi.FieldInfo.GetCustomAttribute<AvroAliasAttribute>()?.AliasList;

            if (aliasAttr != null)
            {
                efi.AliasList.AddRange(aliasAttr);
            }


            var docValueAttr = efi.FieldInfo.GetCustomAttribute<AvroDocAttribute>()?.DocValue;

            if (!string.IsNullOrWhiteSpace(docValueAttr))
            {
                efi.AvroDocValue = docValueAttr;
                efi.HasDocValue = true;
            }


            var nsValueAttr = efi.FieldInfo.GetCustomAttribute<AvroNamespaceAttribute>()?.NamespaceValue;

            if (!string.IsNullOrWhiteSpace(nsValueAttr))
            {
                efi.AvroNameSpace = nsValueAttr;
                efi.HasNamespace = true;
            }

            logger.LogIt(LogSeverity.Verbose, "i18n::Field {0} determined as {1}", efi.FieldName, efi.AvroType);

        }


        private void SetTypeCodeForPrimitives(ExtendedFieldInfo efi)
        {
            switch (efi.TypeCode)
            {
                case TypeCode.Int32:
                    efi.AvroType = efi.FieldInfo.FieldType.BaseType == typeof(Enum) ? AvroFieldType.Enum : AvroFieldType.Int;
                    break;
                case TypeCode.Int64:
                    efi.AvroType = AvroFieldType.Long;
                    break;
                case TypeCode.Single:
                    efi.AvroType = AvroFieldType.Float;
                    break;
                case TypeCode.Double:
                    efi.AvroType = AvroFieldType.Double;
                    break;
                case TypeCode.String:
                    efi.AvroType = AvroFieldType.String;
                    break;
                case TypeCode.Decimal:
                    efi.AvroType = AvroFieldType.Logical;
                    break;
                case TypeCode.Boolean:
                    efi.AvroType = AvroFieldType.Boolean;
                    break;
            }
        }
    }
}
