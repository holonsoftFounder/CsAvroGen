using System;
using System.Collections.Generic;

namespace CsAvroGen.DomainModel.AvroAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Field)]
    public class AvroAliasAttribute : Attribute
    {
        public List<string> AliasList { get; } = new List<string>();

        public AvroAliasAttribute(List<string> aliasList)
        {
            AliasList.AddRange(aliasList);
        }
    }
}
