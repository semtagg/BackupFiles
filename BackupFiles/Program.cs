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
            // var builder = new ConfigurationBuilder()
            //     .AddJsonFile("config.json", true, true);
            // var config = builder.Build();

            // string fileName = "WeatherForecast.json";
            // using FileStream openStream = File.OpenRead(fileName);
            // WeatherForecast? weatherForecast = 
            //     await JsonSerializer.DeserializeAsync<WeatherForecast>(openStream);

            var fileName = "config.json";
            var jsonString = File.ReadAllText(fileName);
            var paths = JsonSerializer.Deserialize<Paths>(jsonString);

            // if (paths != null)
            // {
            //     Console.WriteLine(paths.SourceDirectoryPath);
            //     Console.WriteLine(paths.TargetDirectoryPath);
            // }

            // string fileName = "test.txt";
            var sourcePath = paths.SourceDirectoryPath;
            var targetPath = paths.TargetDirectoryPath;
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
                string[] files = Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var name = Path.GetFileName(s);
                    var path = Path.Combine(targetDir, name);
                    File.Copy(s, path, true);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

            // Keep console window open in debug mode.
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static void CopyFiles()
        {
        }
    }
}
