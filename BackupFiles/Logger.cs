using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BackupFiles
{
    public static class Logger
    {
        public static string Mode { get; set; } = "Debug";
        public static StreamWriter File { get; set; }

        public static void Init()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(File));
            Trace.AutoFlush = true;
        }

        public static void WriteDebug(string message)
        {
            
            Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff",CultureInfo.InvariantCulture) + "  [DEBUG]  " + message);
        }

        public static void WriteInfo(string message)
        {
            if (Mode != "Debug")
            {

                Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture) + "  [INFO]  " + message);
            }
        }

        public static void WriteError(string message)
        {
            if (Mode == "Error")
            {

                Trace.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff",CultureInfo.InvariantCulture) + "  [ERROR]  " + message);
            }
        }
    }
}
