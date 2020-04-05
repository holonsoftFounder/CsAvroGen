using System;
using System.IO;
using System.Reflection;
using CsAvroGen.DomainModel;


namespace holonsoft.CsAvroGen.Executer
{
    internal class TypeInspector
    {
        internal void InspectCompiledType(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            if (!File.Exists(prgArgs.AssemblyName))
            {
                throw new FileNotFoundException(prgArgs.AssemblyName);
            }

            typeInfoData.FullAssemblyPath = Path.GetFullPath(prgArgs.AssemblyName);

            typeInfoData.Assembly = Assembly.LoadFile(typeInfoData.FullAssemblyPath);


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

            InspectCompiledType(typeInfoData);
        }


        internal void InspectCompiledType(TypeInfoData typeInfoData)
        {
            typeInfoData.Namespace = typeInfoData.InspectedType.Namespace;
            
            foreach (var field in typeInfoData.InspectedType.GetFields())
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic) continue;

                var efi = new ExtendedFieldInfo(field);
                typeInfoData.FieldList.Add(efi);

                if (efi.FieldInfo.FieldType.IsClass)
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

                if (subEfi.FieldInfo.FieldType.IsClass)
                {
                    AddSubFields(efi);
                }
            }
        }
    }
}