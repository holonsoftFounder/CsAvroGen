using System;

namespace CsAvroGen.Abstractions.AvroAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AvroNamespaceAttribute : Attribute
    {
        public string NamespaceValue { get; }

        public AvroNamespaceAttribute(string namespaceValue)
        {
            NamespaceValue = namespaceValue;
        }
    }
}
