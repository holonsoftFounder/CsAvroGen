using CsAvroGen.DomainModel;
using System.Configuration;
using System.IO;
using Microsoft.CodeAnalysis;

namespace holonsoft.CsAvroGen.Executer
{
    internal class ConfigReader
    {
        internal void Read(TypeInfoData typeInfoData)
        {
            if (!File.Exists(@".\CsAvroGen.dll.config")) return;

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (!key.StartsWith("MetadataReference::Load")) continue;

                var v = ConfigurationManager.AppSettings.Get(key);

                if (string.IsNullOrWhiteSpace(v)) continue;

                typeInfoData.MetadataReferenceList.Add(MetadataReference.CreateFromFile(v));
            }
        }
    }
}