namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            new Backup("Configuration\\config.json").Start();
        }
    }
}
