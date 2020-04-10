using System;

namespace CsAvroGen.DomainModel.AvroAttributes
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
