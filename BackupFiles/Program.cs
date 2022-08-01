using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileName = "config.json";
            var jsonString = File.ReadAllText(fileName);
            var configuration = JsonSerializer.Deserialize<Configuration>(jsonString);


            if (configuration != null)
            {
                Console.WriteLine(configuration.SourceDirectoryPath);
                Console.WriteLine(configuration.TargetDirectoryPath);
            }

            // string fileName = "test.txt";
            var sourcePath = configuration.SourceDirectoryPath;
            var targetPath = configuration.TargetDirectoryPath;
            var currentDate = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            Console.WriteLine(currentDate);
            // Use Path class to manipulate file and directory paths.
            // var sourceFile = Path.Combine(sourcePath, fileName);
            // var destFile = Path.Combine(targetPath, fileName);

            var targetDir = Path.Combine(targetPath, currentDate);

            // To copy a folder's contents to a new location:
            // Create a new target folder.
            // If the directory already exists, this method does not create a new directory.
            Directory.CreateDirectory(targetDir);

            var logFileName = currentDate + ".log";
            var logPath = Path.Combine(targetDir, logFileName);
            StreamWriter logFile = File.CreateText(logPath);

            Logger.File = logFile;
            Logger.Mode = configuration.LogLevel;
            Logger.Init();

            /*Trace.Listeners.Add(new TextWriterTraceListener(logFile));
            Trace.AutoFlush = true;
            Trace.WriteLine("Starting Backup Log");
            Trace.WriteLine($"Application started {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");*/

            Logger.WriteInfo("Starting Backup Log.");
            Logger.WriteInfo("Application started.");


            // To copy a file to another location and
            // overwrite the destination file if it already exists.
            // File.Copy(sourceFile, destFile, true);

            // To copy all the files in one directory to another directory.
            // Get the files in the source folder. (To recursively iterate through
            // all subfolders under the current directory, see
            // "How to: Iterate Through a Directory Tree.")
            // Note: Check for target path was performed previously
            //       in this code example.
            if (Directory.Exists(sourcePath))
            {
                Logger.WriteDebug("All right! Directory exists. Start copying...");
                string[] files = Directory.GetFiles(sourcePath);

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
