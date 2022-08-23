using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace BackupFiles
{
    public class Backup
    {
        private readonly string _fileName;
        private Logger _logger;

        public Backup(string fileName)
        {
            _fileName = fileName;
        }

        private void SetUpLogger(StreamWriter logFile)
        {
            _logger = new Logger(logFile, Configuration.LogLevel);

            _logger.WriteLog("Application started.", LoggerLevel.Info);
            _logger.WriteLog("Starting Backup Log.", LoggerLevel.Debug);
        }

        private bool IsDirectoryExist(string path)
        {
            if (Directory.Exists(path))
            {
                _logger.WriteLog($"All right! Directory {path} exists.", LoggerLevel.Info);
                _logger.WriteLog($"Start copying from {path}.", LoggerLevel.Debug);

                return true;
            }

            _logger.WriteLog($"Copying is not possible. Directory {path} does not exist.", LoggerLevel.Error);

            return false;
        }

        private void CreateZipArchive()
        {
            ZipFile.CreateFromDirectory(Configuration.TargetDirectory,
                Configuration.TargetDirectory + ".zip",
                CompressionLevel.Fastest,
                true);

            Directory.Delete(Configuration.TargetDirectory, true);
        }

        private void TryCopyFile(string sourceFile, string destFile)
        {
            try
            {
                File.Copy(sourceFile, destFile, true);
                _logger.WriteLog($"LogFile {sourceFile} successfully copied in {destFile}.", LoggerLevel.Debug);
            }
            catch (IOException e)
            {
                _logger.WriteLog($"LogFile {destFile} not copied. Error: {e.Message}", LoggerLevel.Error);
            }
        }

        private void CopyFilesFromDirectory(string directoryPath)
        {
            try
            {
                var shortPath = Path.Combine(Configuration.TargetDirectory,
                    Configuration.GetShortPath(directoryPath));
                Directory.CreateDirectory(shortPath);
                var files = Directory.GetFiles(directoryPath);
                foreach (var f in files)
                {
                    var name = Configuration.GetShortPath(f);
                    var path = Path.Combine(Configuration.TargetDirectory, name);
                    TryCopyFile(f, path);
                }

                _logger.WriteLog($"Copying from {directoryPath} completed.", LoggerLevel.Info);
            }
            catch (IOException e)
            {
                _logger.WriteLog($"Copying from {directoryPath} was incomplete. Error message: {e.Message}",
                    LoggerLevel.Error);
            }
        }

        public void Start()
        {
            using (var logFile = Configuration.SetUp(_fileName))
            {
                SetUpLogger(logFile);

                var directoryQueue = new Queue<string>(Configuration.SourceDirectory);
                while (directoryQueue.Count > 0)
                {
                    var directoryPath = directoryQueue.Dequeue();
                    if (IsDirectoryExist(directoryPath))
                    {
                        CopyFilesFromDirectory(directoryPath);

                        var directories = Directory.GetDirectories(directoryPath);
                        foreach (var directory in directories)
                        {
                            directoryQueue.Enqueue(directory);
                        }
                    }
                }

                _logger.WriteLog("Application completed.", LoggerLevel.Info);
            }

            CreateZipArchive();
        }
    }
}
