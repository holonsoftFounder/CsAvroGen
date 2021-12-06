namespace CsAvroGen.Abstractions.Enums
{
    public enum AvroFieldType
    {
        Undefined,

        Null,
        Boolean,
        Int,
        Long,
        Float,
        Double,
        String,

        Record,
        Enum,
        
        Array,
        ArrayWithRecordType,
        
        Map,
        MapWithRecordType,

        Fixed,

        Logical,
    }
}