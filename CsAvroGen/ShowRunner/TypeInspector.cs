using System;
using System.IO;
using System.Reflection;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.AvroAttributes;
using CsAvroGen.DomainModel.Enums;

namespace holonsoft.CsAvroGen.ShowRunner
{
    internal class TypeInspector
    {
        private TypeInfoData _typeInfoData;

        private readonly FieldInspector _fieldInspector = new FieldInspector();


        internal void InspectFileBasedCompiledType(ProgramArgs prgArgs, TypeInfoData typeInfoData)
        {
            _typeInfoData = typeInfoData;

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
            _typeInfoData = typeInfoData;

            foreach (var t in typeInfoData.Assembly.GetExportedTypes())
            {
                if (!t.Name.Equals(prgArgs.TypeName)) continue;

                typeInfoData.InspectedType = t;
                break;
            }

            if (typeInfoData.InspectedType == null)
            {
                _typeInfoData.Logger.LogIt(LogSeverity.Fatal, "i18n::Cannot load/find type {0}", prgArgs.TypeName);
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

                _fieldInspector.Inspect(typeInfoData.Logger, efi);

                if (efi.AvroType == AvroFieldType.Record || efi.AvroType == AvroFieldType.ArrayWithRecordType || efi.AvroType == AvroFieldType.MapWithRecordType)
                {
                    AddSubFields(efi);
                }
            }
        }


        private void AddSubFields(ExtendedFieldInfo efi)
        {

            var fields = efi.AvroType == AvroFieldType.Record ? efi.FieldInfo.FieldType.GetFields() : efi.ComplexArrayOrMapType?.GetFields();

            if (fields == null) return;

            foreach (var field in fields)
            {
                if (field.IsInitOnly || field.IsLiteral || field.IsStatic) continue;

                var subEfi = new ExtendedFieldInfo(field);
                efi.SubFieldList.Add(subEfi);

                _fieldInspector.Inspect(_typeInfoData.Logger, subEfi);

                if (subEfi.AvroType == AvroFieldType.Record || efi.AvroType == AvroFieldType.ArrayWithRecordType || efi.AvroType == AvroFieldType.MapWithRecordType)
                {
                    AddSubFields(subEfi);
                }
            }
        }
    }
}