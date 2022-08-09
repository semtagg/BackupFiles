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

                Logger.WriteLog("Application started.", LoggerLevel.Info);
                Logger.WriteLog("Starting Backup Log.", LoggerLevel.Debug);

                var queue = new Queue<string>(Configuration.SourceDirectory);
                while (queue.Count > 0)
                {
                    var s = queue.Dequeue();
                    if (Directory.Exists(s))
                    {
                        Logger.WriteLog($"All right! Directory {s} exists.", LoggerLevel.Info);
                        Logger.WriteLog($" Start copying from {s}.", LoggerLevel.Debug);

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
                                Logger.WriteLog($"LogFile {f} successfully copied in {path}.", LoggerLevel.Debug);
                            }
                            catch (IOException e)
                            {
                                Logger.WriteLog($"LogFile {path} not copied. Error: {e.Message}", LoggerLevel.Error);
                            }
                        }

                        var directories = Directory.GetDirectories(s);
                        foreach (var directory in directories)
                        {
                            queue.Enqueue(directory);
                        }

                        Logger.WriteLog($"Copying from {s} completed.", LoggerLevel.Info);
                    }
                    else
                    {
                        Logger.WriteLog("Copying is not possible. Directory does not exist.", LoggerLevel.Error);
                    }
                }

                Logger.WriteLog("Application completed.", LoggerLevel.Info);
            }
        }
    }
}
