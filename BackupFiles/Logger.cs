using System;
using System.Diagnostics;
using System.IO;

namespace BackupFiles
{
    public static class Logger
    {
        public static string Mode { get; set; } = "Debug";
        public static StreamWriter File { get; set; }

        public static void WriteDebug(string message)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(File));
            Trace.AutoFlush = true;
            Trace.WriteLine( "[DEBUG]" + message);
        }

        public static void WriteInfo(string message)
        {
            if (Mode != "Debug")
            {
                Trace.Listeners.Add(new TextWriterTraceListener(File));
                Trace.AutoFlush = true;
                Trace.WriteLine( "[INFO]" + message);
            }
        }

        public static void WriteError(string message)
        {
            if (Mode == "Error")
            {
                Trace.Listeners.Add(new TextWriterTraceListener(File));
                Trace.AutoFlush = true;
                Trace.WriteLine( "[ERROR]" + message);
            }
        }
    }
}
