using System;
using System.Collections.Generic;
using System.Reflection;

namespace CsAvroGen.DomainModel
{
    public class ExtendedFieldInfo
    {
        public FieldInfo FieldInfo { get; }
        public string FieldName => FieldInfo.Name;
        
        public TypeCode TypeCode { get; }
        public bool IsNullable { get; }
        public bool HasDefaultValue { get; set; }
        public bool IsArray => FieldInfo.FieldType.IsArray;

        public object AvroDefaultValue { get; }

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



            AvroDefaultValue = fi.GetCustomAttribute<AvroDefaultValueAttribute>()?.DefaultValue;

            HasDefaultValue = AvroDefaultValue != null;
        }
    }
}