using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.Enums;

namespace holonsoft.CsAvroGen.ShowRunner
{
    internal class ConfigReader
    {
        internal void Read(TypeInfoData typeInfoData)
        {
            typeInfoData.IndentFactor = 2;

            var configFileName = Assembly.GetEntryAssembly().Location + ".config";

            if (!File.Exists(configFileName)) return;

            typeInfoData.Logger.LogIt(LogSeverity.Verbose, "i18n::Reading config from " + configFileName);

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key.StartsWith("MetadataReference::Load", StringComparison.InvariantCultureIgnoreCase))
                {
                    var v = ConfigurationManager.AppSettings.Get(key);

                    if (string.IsNullOrWhiteSpace(v)) continue;

                    typeInfoData.MetadataReferenceList.Add(MetadataReference.CreateFromFile(v));
                }

                if (string.Compare(key, "IndentFactor", StringComparison.InvariantCultureIgnoreCase) == 0)
                {
                    var v = ConfigurationManager.AppSettings.Get(key);

                    typeInfoData.IndentFactor = int.TryParse(v, out var result) ? result : 2;
                }


                if (string.Compare(key, "I18N", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    typeInfoData.Logger.Locale = ConfigurationManager.AppSettings.Get(key);
                }
            }


            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.FullName.Contains("netstandard", StringComparison.InvariantCultureIgnoreCase))
                {
                    typeInfoData.MetadataReferenceList.Add(MetadataReference.CreateFromFile(a.Location));
                }
            }
        }
    }
}