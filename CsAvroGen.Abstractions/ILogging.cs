using CsAvroGen.Abstractions.Enums;

namespace CsAvroGen.Abstractions
{
    public interface ILogging
    {
        public string Locale { get; set; }

        public void LogIt(LogSeverity severity, string msg);
        public void LogIt(LogSeverity severity, string msg, object p1);
        public void LogIt(LogSeverity severity, string msg, object p1, object p2);
        public void LogIt(LogSeverity severity, string msg, object p1, object p2, object p3);
    }
}