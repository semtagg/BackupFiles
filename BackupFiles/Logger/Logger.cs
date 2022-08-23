using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BackupFiles
{
    public class Logger
    {
        private readonly LoggerLevel _mode;
        
        public Logger(StreamWriter logFile, LoggerLevel mode)
        {
            _mode = mode;
            Trace.Listeners.Add(new TextWriterTraceListener(logFile));
            Trace.AutoFlush = true;
        }

        public void WriteLog(string message, LoggerLevel type)
        {
            if (_mode >= type)
            {
                Trace.WriteLine(GetDate()  + $"  [{type}]  " + message);
            }
        }

        private string GetDate()
            => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
    }
}
