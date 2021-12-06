namespace CsAvroGen.Abstractions.Enums
{
    public enum ReturnCode
    {
        None = 0,
        Ok = 0,

        NothingToDo = 1,
        TypeCompilationFailed = 2,
        TypeInspectionFailed = 3,
        SchemaGenerationFailed = 4,

    }
}