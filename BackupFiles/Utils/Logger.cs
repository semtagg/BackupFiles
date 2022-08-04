using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BackupFiles
{
    public static class Logger
    {
        public static string Mode { get; set; } = "Debug";
        public static StreamWriter LogFile { get; set; }

        public static void Initialize()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(LogFile));
            Trace.AutoFlush = true;
        }

        public static void WriteDebug(string message)
        {
            Trace.WriteLine(Write(message, LogEventTypes.Debug));
        }

        public static void WriteInfo(string message)
        {
            if (Mode != "Debug")
            {
                Trace.WriteLine(Write(message, LogEventTypes.Info));
            }
        }

        public static void WriteError(string message)
        {
            if (Mode == "Error")
            {
                Trace.WriteLine(Write(message, LogEventTypes.Error));
            }
        }

        private static string Write(string message, string type)
            => GetDate() + $"  [{type}]  " + message;

        private static string GetDate()
            => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
    }
}
