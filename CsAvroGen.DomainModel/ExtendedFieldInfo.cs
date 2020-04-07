using System;
using System.Collections.Generic;
using System.Reflection;
using CsAvroGen.DomainModel.AvroAttributes;

namespace CsAvroGen.DomainModel
{
    public class ExtendedFieldInfo
    {
        public FieldInfo FieldInfo { get; }
        public string FieldName => FieldInfo.Name;
        
        public TypeCode TypeCode { get; }
        public bool HasDefaultValue { get; }
        public bool HasDocValue { get; }

        public bool IsNullable { get; }
        public bool IsMap { get; }
        public bool IsArray => FieldInfo.FieldType.IsArray;
        public bool IsClass => FieldInfo.FieldType.IsClass;
        public bool HasAlias => AliasList.Count > 0;


        public string ImplementingClassName { get; }

        public string AvroDocValue { get; }
        public object AvroDefaultValue { get; }
        public List<string> AliasList { get; } = new List<string>();


        public List<ExtendedFieldInfo> SubFieldList { get; } = new List<ExtendedFieldInfo>();

        public ExtendedFieldInfo(FieldInfo fi)
        {
            FieldInfo = fi;
            
            var fieldType = FieldInfo.FieldType;
            TypeCode = Type.GetTypeCode(fieldType);

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var t = Nullable.GetUnderlyingType(fieldType);

                if (t != null)
                {
                    TypeCode = Type.GetTypeCode(t);
                    IsNullable = true;
                }
            }

            if (fieldType.IsArray)
            {
                TypeCode = Type.GetTypeCode(fieldType.GetElementType());
            }


            if (fieldType.IsGenericType)
            {
                var underlyingName = fieldType.UnderlyingSystemType.Name;

                IsMap = underlyingName.Contains("Dictionary", StringComparison.InvariantCultureIgnoreCase)
                        || underlyingName.Contains("SortedList", StringComparison.InvariantCultureIgnoreCase)
                        || underlyingName.Contains("SortedDictionary", StringComparison.InvariantCultureIgnoreCase);
            }

            if (fieldType.IsClass)
            {
                ImplementingClassName = fieldType.Name;
            }
            

            AvroDefaultValue = fi.GetCustomAttribute<AvroDefaultValueAttribute>()?.DefaultValue;
            HasDefaultValue = AvroDefaultValue != null;


            var aliasAttr = fi.GetCustomAttribute<AvroAliasAttribute>()?.AliasList;

            if (aliasAttr != null)
            {
                AliasList.AddRange(aliasAttr);
            }


            var docValueAttr = fi.GetCustomAttribute<AvroDocAttribute>()?.DocValue;

            if (!string.IsNullOrWhiteSpace(docValueAttr))
            {
                AvroDocValue = docValueAttr;
                HasDocValue = true;
            }
        }
    }
}