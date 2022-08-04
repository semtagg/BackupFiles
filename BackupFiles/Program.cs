using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.SetUp("config.json");
            
            var sourcePath = Configuration.GetSourcePaths();
            var targetPath = Configuration.GetTargetPaths();
            var currentDate = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            
            var targetDir = Path.Combine(targetPath, currentDate);
            Directory.CreateDirectory(targetDir);

            var logFileName = currentDate + ".log";
            var logPath = Path.Combine(targetDir, logFileName);
            StreamWriter logFile = File.CreateText(logPath);

            Logger.File = logFile;
            Logger.Mode = Configuration.GetLoggingLevel();
            Logger.Init();

            Logger.WriteInfo("Starting Backup Log.");
            Logger.WriteInfo("Application started.");


            if (Directory.Exists(sourcePath[0]))
            {
                Logger.WriteDebug("All right! Directory exists. Start copying...");
                string[] files = Directory.GetFiles(sourcePath[0]);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var name = Path.GetFileName(s);
                    var path = Path.Combine(targetDir, name);
                    try
                    {
                        File.Copy(s, path, true);
                        Logger.WriteDebug($"File {s} successfully copied in {path}.");
                    }
                    catch (IOException e)
                    {
                        Logger.WriteError($"File {path} not copied. Error: {e.Message}");
                    }
                }

                Logger.WriteError("Copying completed.");
            }
            else
            {
                Logger.WriteError("Copying is not possible. Directory does not exist.");
            }

            Logger.WriteInfo("Application completed.");
        }
    }
}
