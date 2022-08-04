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
        public static string LogLevel => _configurationModel.LogLevel;

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

        private static string GetDate()
            => DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
    }
}
