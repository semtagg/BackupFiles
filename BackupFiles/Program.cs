using System;
using System.Collections.Generic;
using System.IO;

namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            DoWork("Configuration\\config.json");
        }

        private static void DoWork(string fileName)
        {
            using (var logFile = Configuration.SetUp(fileName))
            {
                Logger.LogFile = logFile;
                Logger.Mode = Configuration.LogLevel;
                Logger.Initialize();

                Logger.WriteInfo("Application started.");
                Logger.WriteDebug("Starting Backup Log.");

                var queue = new Queue<string>(Configuration.SourceDirectory);
                while (queue.Count > 0)
                {
                    var s = queue.Dequeue();
                    if (Directory.Exists(s))
                    {
                        Logger.WriteInfo($"All right! Directory {s} exists.");
                        Logger.WriteDebug($" Start copying from {s}.");

                        var temp = Configuration.TargetDirectory
                                   + Configuration.GetShortPath(s);
                        Directory.CreateDirectory(Path.Combine(Configuration.TargetDirectory, Configuration.GetShortPath(s)));
                        var files = Directory.GetFiles(s);
                        Console.WriteLine(Directory.GetDirectoryRoot(s));
                        foreach (var f in files)
                        {
                            var name = Configuration.GetShortPath(f);
                            var path = Path.Combine(Configuration.TargetDirectory , name);
                            try
                            {
                                File.Copy(f, path, true);
                                Logger.WriteDebug($"LogFile {f} successfully copied in {path}.");
                            }
                            catch (IOException e)
                            {
                                Logger.WriteError($"LogFile {path} not copied. Error: {e.Message}");
                            }
                        }

                        var directories = Directory.GetDirectories(s);
                        foreach (var directory in directories)
                        {
                            queue.Enqueue(directory);
                        }

                        Logger.WriteInfo($"Copying from {s} completed.");
                    }
                    else
                    {
                        Logger.WriteError("Copying is not possible. Directory does not exist.");
                    }
                }

                Logger.WriteInfo("Application completed.");
            }
        }
    }
}
