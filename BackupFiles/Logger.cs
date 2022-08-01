using System;
using System.Diagnostics;
using System.IO;

namespace BackupFiles
{
    public enum LoggerLevels
    {
        Debug,
        Info,
        Error
    }

    public static class Logger
    {
        public static LoggerLevels Mode { get; set; } = LoggerLevels.Debug;
        public static StreamWriter File { get; set; }

        public static void WriteDebug(string message)
        {
        }

        public static void WriteInfo(string message)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(File));
            Trace.AutoFlush = true;
            Trace.WriteLine(message);
        }

        public static void WriteError(string message)
        {
        }
    }
}
