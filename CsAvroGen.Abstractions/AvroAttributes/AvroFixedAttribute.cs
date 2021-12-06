using System;

namespace CsAvroGen.Abstractions.AvroAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AvroFixedAttribute : Attribute
    {
        public int Size { get; }
        public string DataClassName { get; }

        public AvroFixedAttribute(string name, int size)
        {
            Size = size;
            DataClassName = name;
        }
    }
}
