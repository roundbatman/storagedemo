using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Service;
using StorageMangler.Domain.Utilities;
using StorageMangler.Infrastructure;

namespace StorageMangler.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            try
            {
                var serviceCollection = ConfigureServices(services);
                var serviceProvider = serviceCollection.BuildServiceProvider();
                Console.WriteLine("Services are configured");
                Console.WriteLine("Starting application..");
                await serviceProvider.GetService<Application>().Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        private static IServiceCollection ConfigureServices(IServiceCollection services)
        {
            //            var configFile = Environment.GetEnvironmentVariable($"AZ_BATCH_APP_PACKAGE_{PackageName}#{PackageVer}") + 
            //                             $"\\{PackageName}\\appsettings.json";

            var localConfigExists = File.Exists("appsettings.Local.json");
            Console.WriteLine(localConfigExists
                ? "Using local config from appsettings.Local.json"
                : "Using purely environment, no local config file appsettings.Local.json present");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
            if (localConfigExists)
            {
                builder = builder.AddJsonFile("appsettings.Local.json", true, true);
            }

            var configuration = builder.Build();

            services.AddLogging(
                loggingBuilder => loggingBuilder.ClearProviders()
                    .AddConsole(options => options.TimestampFormat = "[HH:mm:ss] ")
                    .SetMinimumLevel(LogLevel.Information).AddFilter(Filter));
            
            services.AddSingleton<ITableClientFactory>(x => 
                new TableClientFactory(configuration["tableStorageConString"]));
            services.AddSingleton<IDateTimeFactory, DateTimeFactory>();
            services.AddSingleton<IForbiddenPatternsRepository, ForbiddenPatternsRepository>();
            services.AddSingleton<ForbiddenNamesService>();
            services.AddSingleton<IFileMetaDataRepository, FileMetadataRepository>();
            services.AddSingleton<IFileStorage>( x =>
                new FileBlobStorage(configuration["blobStorageConString"]));
            services.AddSingleton<Application>();

            services.AddSingleton<IStorageService, StorageService>();

            return services;
        }
        
        private static bool Filter(string provider, string category, LogLevel logLevel)
        {
            if (provider.Contains("ConsoleLoggerProvider")
                && logLevel >= LogLevel.Information)
            {
                return true;
            }
            return false;
        }
    }
}