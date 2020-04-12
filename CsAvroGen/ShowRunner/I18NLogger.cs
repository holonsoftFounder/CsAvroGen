using System;
using System.IO;
using System.Reflection;
using System.Resources;
using CsAvroGen.DomainModel;
using CsAvroGen.DomainModel.Enums;


namespace holonsoft.CsAvroGen.ShowRunner
{
    internal class I18NLogger: ILogging
    {
        private ProgramArgs _prgArgs;

        private ResourceManager _resMgr;
        private bool _resLoadAttempt;

        public string Locale { get; set; }

        public I18NLogger(ProgramArgs prgArgs)
        {
            _prgArgs = prgArgs;

        }

        /// <inheritdoc />
        public void LogIt(LogSeverity severity, string msg)
        {
            if (severity < _prgArgs.LogLevel) return;

            Console.WriteLine(Translate(msg));
        }


        /// <inheritdoc />
        public void LogIt(LogSeverity severity, string msg, object p1)
        {
            if (severity < _prgArgs.LogLevel) return;

            LogIt(severity, string.Format(Translate(msg), p1));
        }
        
        /// <inheritdoc />
        public void LogIt(LogSeverity severity, string msg, object p1, object p2)
        {
            if (severity < _prgArgs.LogLevel) return;

            LogIt(severity, string.Format(Translate(msg), p1, p2));
        }

        /// <inheritdoc />
        public void LogIt(LogSeverity severity, string msg, object p1, object p2, object p3)
        {
            if (severity < _prgArgs.LogLevel) return;

            LogIt(severity, string.Format(Translate(msg), p1, p2, p3));
        }


        private string Translate(string msg)
        {
            if (!msg.StartsWith("i18n::", StringComparison.InvariantCultureIgnoreCase))
            {
                return msg;
            }

            SafeLoadResourceFile();

            var s = msg.Replace("i18n::", string.Empty);

            if (_resMgr == null)
            {
                return (s);
            }

            var result = _resMgr.GetString(s);

            return string.IsNullOrWhiteSpace(result) ? msg : result;
        }



        private void SafeLoadResourceFile()
        {
            if (_resLoadAttempt) return;

            try
            {
                var baseName = ("CsAvroGen.I18N." + Locale + ".Resources").Replace('-', '_');
                var assemblyName = Path.GetFullPath("CsAvroGen.I18N." + Locale + ".dll");

                var assembly = Assembly.LoadFile(assemblyName);

                _resMgr = new ResourceManager(baseName, assembly);
            }
            catch 
            {
            }

            _resLoadAttempt = true;
        }
    }
}
