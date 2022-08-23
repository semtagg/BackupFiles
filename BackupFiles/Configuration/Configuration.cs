using System;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace BackupFiles
{
    public static class Configuration
    {
        private static ConfigurationModel _configurationModel;
        public static string[] SourceDirectory => _configurationModel.SourceDirectoryPath;
        public static string TargetDirectory { get; private set; }
        public static LoggerLevel LogLevel => _configurationModel.LogLevel;

        public static StreamWriter SetUp(string fileName)
        {
            var jsonString = File.ReadAllText(fileName);
            _configurationModel = JsonSerializer.Deserialize<ConfigurationModel>(jsonString);

            var currentDate = GetDateTime();
            TargetDirectory = Path.Combine(_configurationModel.TargetDirectoryPath, currentDate);
            Directory.CreateDirectory(TargetDirectory);

            var logFileName = currentDate + ".log";
            var logPath = Path.Combine(TargetDirectory, logFileName);
            return File.CreateText(logPath);
        }

        public static string GetShortPath(string path)
        {
            foreach (var sd in SourceDirectory)
            {
                if (HasSubdirectory(sd, path))
                {
                    var temp = sd.Split("\\");
                    return temp[^1] + path[sd.Length..];
                }
            }

            return null;
        }

        private static bool HasSubdirectory(string source, string isSubdirectory)
        {
            var sourceArray = source.Split("\\");
            var isSubdirectoryArray = isSubdirectory.Split("\\");

            for (int k = 0; k < sourceArray.Length; k++)
            {
                if (sourceArray[k] != isSubdirectoryArray[k]) 
                    return false;
            }

            return true;
        }

        private static string GetDateTime()
            => DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
