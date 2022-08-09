using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

            var currentDate = GetDate();
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
                if (IsIn(sd, path))
                {
                    var temp = sd.Split("\\");
                    return temp[^1] + path[sd.Length..];
                }
            }

            return null;
        }

        private static bool IsIn(string i, string j)
        {
            var a = i.Split("\\");
            var b = j.Split("\\");

            for (int k = 0; k < a.Length; k++)
            {
                if (a[k] != b[k]) 
                    return false;
            }

            return true;
        }

        private static string GetDate()
            => DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
