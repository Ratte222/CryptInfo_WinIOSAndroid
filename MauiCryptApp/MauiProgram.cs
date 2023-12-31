using Backuper_Core.Configurations;
using Backuper_Core.Helpers;
using Backuper_Core.Services;
using Backuper_Core.Services.ServiceBuilders;
using Backuper_Mega.Helpers;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Services;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using MauiCryptApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Reflection;

namespace MauiCryptApp
{
    public static class MauiProgram
    {
        public static IServiceScope ServiceScope { get; set; }
        const string mainCryptFileName = "Crypt_Main";
        const string fileInfosFileName = "FileInfos.json";
        public const string backupSettingFileName = "backupSettings.json";
        public static string BackupSettingFullPath { 
            get { return Path.Combine(FileSystem.Current.AppDataDirectory, backupSettingFileName); } }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            AppSettings appSettings = new AppSettings()
            {
                DirCryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
                {
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "Crypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, mainCryptFileName),
                    },
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "TestCrypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestCrypt")
                    }
                }),
                selected_crypr_file = "Crypt"
            };
            builder.Services.AddSingleton<IAppSettings>(appSettings);
            SearchSettings searchSettings = new SearchSettings()
            {
                CaseSensitive = false,
                SearchEverywhere = false,
                SearchInHeader = false,
                SearchInTegs = false,
                SearchUntilFirstMatch = false,
                ViewServiceInformation = false
            };
            builder.Services.AddSingleton<ISearchSettings>(searchSettings);
            Dictionary<string, string> backupsPath = new();
            backupsPath.Add("0", $"sync/{mainCryptFileName}");
            Dictionary<string, string> savedFilePath = new();
            savedFilePath.Add("0", appSettings.SelectedCryptFile.Path);
            string fileInfosFileFullPath = Path.Combine(FileSystem.Current.AppDataDirectory, fileInfosFileName);

            //var dictionaryFileBuilders = new ConcurrentDictionary<string, Type>();//BuildersDIHelper.GetDictionaryForFileServiceBuilders();
            //dictionaryFileBuilders.TryAdd(ConfigureServiceHelper.LocalStorageShortProviderName, typeof(LocalFileServiceBuilder));
            //dictionaryFileBuilders.TryAdd(ConfigureServiceHelper.AWSS3ShortProviderName, typeof(AWSS3FileServiceBuilder));
            //var dictionaryDirectoryBuilders = new ConcurrentDictionary<string, Type>(); //BuildersDIHelper.GetDictionaryForDirectoryServiceBuilders();
            //dictionaryDirectoryBuilders.TryAdd(ConfigureServiceHelper.LocalStorageShortProviderName, typeof(LocalDirectoryServiceBuilder));
            //dictionaryDirectoryBuilders.TryAdd(ConfigureServiceHelper.AWSS3ShortProviderName, typeof(AWSS3DirectoryServiceBuilder));
            var awsCredentials = new AWSS3Configuration();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //if (assemblies.Any(x=>x.FullName.Contains("Backuper-Mega")))
            //{ 
            //    var temp = Assembly.Load("Backuper-Mega");
            //    assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //}
            if (!BackuperMegaHelper.CheckAssembly(assemblies))
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            var backuperCoreBuildOption = new Backuper_Core.Models.BackuperCoreDIOptionModel()
            {
                aWSS3Configuration = awsCredentials,
                backupSettings = new CBackupSettings(new BackupSetting[]
                {
                    new BackupSetting()
                    {
                        Name = "synchronize",
                        ProviderName_From = "",//ConfigureServiceHelper.AWSS3ShortProviderName,
                        ProviderName_To = "",//ConfigureServiceHelper.LocalStorageShortProviderName,
                        WhatDoWithFile = WhatDoWithFile.CopyIfNewer,
                        RetainedFileCountLimit = 1,
                        SavedFilePaths = savedFilePath,
                        BackupPaths = backupsPath,
                        Credentials = new BackupCredentials()
                        {
                            AWS_S3_BucketName = "backup name",
                            AWS_AccessKeyId = "key id",
                            AWS_SecretAccessKey = "key",
                            AWS_Region = "region"
                        },
                        Cron = "* * * * *"
                    }
                }),
                fileInfosFullPath = fileInfosFileFullPath,
                logger = new MockLogger(),
                //dictionaryFileBuilders = dictionaryFileBuilders,
                //dictionaryDirectoryBuilders = dictionaryDirectoryBuilders
                assemblies = assemblies
            };
            
            builder.Services.RegisterBackuperCore(backuperCoreBuildOption);
            builder.Services.AddScoped<ICryptService, CryptService_Android>();
            builder.Services.AddScoped<IDataStore<Item>, DataStoreService>();
            builder.Services.AddLogging(options =>
            {
                options.AddDebug();
            });
            
            var result = builder.Build();
            ServiceScope = result.Services.CreateScope();
            //var backupJob = ServiceScope.ServiceProvider.GetRequiredService<BackupJob>();
            //backupJob.CreateBackupWithTry(backuperCoreBuildOption.backupSettings.BackupSettings.First());
            return result;
            //return builder.Build();
        }
    }

    public class MockLogger : Microsoft.Extensions.Logging.ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            
        }
    }
}