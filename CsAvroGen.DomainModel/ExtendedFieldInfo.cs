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

        public AvroFieldType AvroType { get; set; } = AvroFieldType.Undefined;

        public TypeCode TypeCode { get; set; }
        public bool HasDefaultValue { get; set; }
        public bool HasDocValue { get; set; }
        public bool HasNamespace { get; set; }

        public bool IsNullable { get; set; }
        public bool IsMap { get; set; }
        public bool IsArray => FieldInfo.FieldType.IsArray;
        public bool IsClass => FieldInfo.FieldType.IsClass;
        public bool HasAlias => AliasList.Count > 0;

        public string ImplementingClassName { get; set; }

        public string AvroNameSpace { get; set; }
        public string AvroDocValue { get; set; }
        public object AvroDefaultValue { get; set; }
        public List<string> AliasList { get; } = new List<string>();


        public List<ExtendedFieldInfo> SubFieldList { get; } = new List<ExtendedFieldInfo>();

        public ExtendedFieldInfo(FieldInfo fi)
        {
            FieldInfo = fi;
        }
    }
}