using System.IO;
using System.Text.Json;

namespace BackupFiles
{
    public static class Configuration
    {
        private static ConfigurationModel _configurationModel;

        public static void SetUp(string fileName)
        {
            var jsonString = File.ReadAllText(fileName);
            _configurationModel = JsonSerializer.Deserialize<ConfigurationModel>(jsonString);
        }

        public static string[] GetSourcePaths()
            => _configurationModel.SourceDirectoryPath;

        
        public static string GetTargetPaths()
            => _configurationModel.TargetDirectoryPath;

        public static string GetLoggingLevel()
            => _configurationModel.LogLevel;
    }
}
