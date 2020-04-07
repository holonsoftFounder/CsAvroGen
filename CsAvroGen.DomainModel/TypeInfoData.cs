using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace CsAvroGen.DomainModel
{
    public class TypeInfoData
    {
        public Type InspectedType { get; set; }

        public Assembly Assembly { get; set; }
        public string FullAssemblyPath { get; set; }
        public string Namespace { get; set; }
        public string DocValue { get; set; }

        public int IndentFactor { get; set; }

        public List<MetadataReference> MetadataReferenceList { get; } = new List<MetadataReference>();

        public List<ExtendedFieldInfo> FieldList { get; } = new List<ExtendedFieldInfo>();
        
    }
}