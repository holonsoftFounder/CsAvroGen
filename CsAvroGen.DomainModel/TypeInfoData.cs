using System;
using System.Collections.Generic;
using System.Reflection;

namespace CsAvroGen.DomainModel
{
    public class TypeInfoData
    {
        public Type InspectedType { get; set; }

        public Assembly Assembly { get; set; }
        public string FullAssemblyPath { get; set; }
        public string Namespace { get; set; }

        public List<ExtendedFieldInfo> FieldList { get; } = new List<ExtendedFieldInfo>();

    }
}