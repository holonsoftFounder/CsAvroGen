﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;

namespace CsAvroGen.Abstractions
{
    public class TypeInfoData
    {
        public ILogging Logger { get; }
        
        public int IndentFactor { get; set; }

        public List<MetadataReference> MetadataReferenceList { get; } = new List<MetadataReference>();

        public Type InspectedType { get; set; }
        public Assembly Assembly { get; set; }
        public string FullAssemblyPath { get; set; }
        public string Namespace { get; set; }
        public string DocValue { get; set; }
        public List<ExtendedFieldInfo> FieldList { get; } = new List<ExtendedFieldInfo>();


        public TypeInfoData(ILogging logger)
        {
            Logger = logger;
        }

    }
}