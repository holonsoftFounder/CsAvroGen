# CsAvroGen
Generate AVRO schema file from an ordinary POCO

For quick readers:

Generate code from a given assemby/type combination
csavrogen.exe -an CsAvroGen.SelftestData.dll -tn SelfTesting -o ".\generated"


Read file, compile it and process type
-f .\DemoFiles\SelfTestingFrom.txt  -tn SelfTestingFromText -o ".\generated"

-h Get help

-v Get version info


Using this geneator you have two ways: 
1. You provide an assembly with type to be inspected. This is for cases you don't get source code of a type
2. You provide a textfile with source code - yeah, you read that right. The source code file(s) will be compiled on the fly and afterwards the generation of AVRO schema will be done. This makes it very easy to test until you get a result that matches your expectations.

Here is a partial example (please find whole example in package):

  // Main template file for AVRO schema generation
  //
  // Include other files to get the complete source code
  // You can either define al in one file or split it to an arbitrary count of files
  //
  //@meta::include ".\Subclass.txt"
  //@meta::include ".\SubSubclass.txt"
  //@meta::include ".\ImportantEnum.txt"


  using System.Collections.Generic;
  using System.Drawing;
  using CsAvroGen.DomainModel.AvroAttributes;

  namespace holonsoft.CsAvroGen.SelfTestData
  {
      [AvroNamespace("ns.created.by.attribute")]
      [AvroDoc("Test class composition for AVRO generation")]
      public class SelfTestingFromText
      {
          [AvroDoc("Field can be null, so we get a AVRO UNION")]
          public int? NullableIntField;

          [AvroDoc("This field has a default value")]
          [AvroDefaultValue(5)]
          public int IntField;

As you can see, you can split source code files and use the //meta::include tag at the beginning of a line to include more files

Fields are controlled by decorating them with attributes.

All important types are supported within this release, but there is still some more work for the next release:

Roadmap - coming soon functionality:
- Add support for logical types
- Add support for ALIAS tag
- Add support for sort order of records

- Add translation support

Hope you find this generator helpful for your work. 
