using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BackupFiles
{
    public static class Logger
    {
        public static LoggerLevel Mode { get; set; } = LoggerLevel.Error;
        public static StreamWriter LogFile { get; set; }

        public static void Initialize()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(LogFile));
            Trace.AutoFlush = true;
        }

        public static void WriteLog(string message, LoggerLevel type)
        {
            if (Mode >= type)
            {
                Trace.WriteLine(GetDate()  + $"  [{type}]  " + message);
            }
        }

        private static string GetDate()
            => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
    }
}
