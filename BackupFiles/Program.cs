using System.IO;

namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            DoWork("config.json");
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

                var sourcePath = Configuration.SourceDirectory;
                foreach (var s in sourcePath)
                {
                    if (Directory.Exists(s))
                    {
                        Logger.WriteInfo($"All right! Directory {s} exists.");
                        Logger.WriteDebug($" Start copying from {s}.");

                        var files = Directory.GetFiles(s);
                        foreach (var f in files)
                        {
                            var name = Path.GetFileName(f);
                            var path = Path.Combine(Configuration.TargetDirectory, name);
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
