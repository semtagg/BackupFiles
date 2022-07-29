using System;
using Microsoft.Extensions.Configuration;

namespace BackupFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("config.json", true, true);
            var config = builder.Build();
            
            var connectionString = config["ConnectionString"];
            var emailHost = config["Smtp:Host"];
            Console.WriteLine($"Connection String is: {connectionString}");
            Console.WriteLine($"Email Host is: {emailHost}");
            Console.ReadLine();
        }
    }
}
