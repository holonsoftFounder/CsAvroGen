using System;
using System.IO;
using System.Reflection;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.AvroAttributes;


namespace holonsoft.CsAvroGen.Executer
{
    internal class TypeInspector
    {
        private readonly FieldInspector _fieldInspector = new FieldInspector();


        internal void InspectFileBasedCompiledType(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            if (!File.Exists(prgArgs.AssemblyName))
            {
                throw new FileNotFoundException(prgArgs.AssemblyName);
            }

            typeInfoData.FullAssemblyPath = Path.GetFullPath(prgArgs.AssemblyName);
            typeInfoData.Assembly = Assembly.LoadFile(typeInfoData.FullAssemblyPath);
            
            InspectCompiledType(prgArgs, typeInfoData);
        }


        
        internal void InspectCompiledType(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            foreach (var t in typeInfoData.Assembly.GetExportedTypes())
            {
                if (!t.Name.Equals(prgArgs.TypeName)) continue;

                typeInfoData.InspectedType = t;
                break;
            }

            if (typeInfoData.InspectedType == null)
            {
                throw new TypeLoadException(prgArgs.TypeName);
            }

            
            typeInfoData.Namespace = typeInfoData.InspectedType.Namespace;
            
            var ns = typeInfoData.InspectedType.GetCustomAttribute<AvroNamespaceAttribute>()?.NamespaceValue;

            if (!string.IsNullOrWhiteSpace(ns))
            {
                typeInfoData.Namespace = ns;
            }

            var doc = typeInfoData.InspectedType.GetCustomAttribute<AvroDocAttribute>()?.DocValue;
            if (!string.IsNullOrWhiteSpace(doc))
            {
                typeInfoData.DocValue = doc;
            }


            foreach (var field in typeInfoData.InspectedType.GetFields())
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic) continue;

                var efi = new ExtendedFieldInfo(field);
                typeInfoData.FieldList.Add(efi);

                _fieldInspector.Inspect(efi);

                if (efi.IsClass && !(efi.IsArray || efi.IsMap))
                {
                    AddSubFields(efi);
                }
            }
        }


        private void AddSubFields(ExtendedFieldInfo efi)
        {
            foreach (var field in efi.FieldInfo.FieldType.GetFields())
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic) continue;

                var subEfi = new ExtendedFieldInfo(field);
                efi.SubFieldList.Add(subEfi);

                _fieldInspector.Inspect(subEfi);

                if (subEfi.IsClass && !(subEfi.IsArray || subEfi.IsMap))
                {
                    AddSubFields(subEfi);
                }
            }
        }
    }
}