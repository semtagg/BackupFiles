namespace BackupFiles
{
    public class ConfigurationModel
    {
        public string[] SourceDirectoryPath { get; set; }
        public string TargetDirectoryPath { get; set; }
        public string LogLevel { get; set; }
    }
}
