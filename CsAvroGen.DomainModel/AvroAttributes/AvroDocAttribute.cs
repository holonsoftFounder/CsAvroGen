using System;

namespace CsAvroGen.DomainModel.AvroAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class AvroDocAttribute : Attribute
    {
        public string DocValue { get; }

        public AvroDocAttribute(string docValue)
        {
            DocValue = docValue;
        }
    }
}
