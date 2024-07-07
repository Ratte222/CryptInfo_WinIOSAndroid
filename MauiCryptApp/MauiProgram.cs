using CommunityToolkit.Maui;
using AuxiliaryLib.Extensions;
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
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Reflection;

namespace MauiCryptApp
{
    public static class MauiProgram
    {
        public static IServiceScope ServiceScope { get; set; }
        public const string MAIN_CRYPT_FILE_NAME = "Crypt_Main";
        public const string FILE_INFOS_FILE_NAME = "FileInfos.json";
        public const string APPLICATION_SETTINGS_FILE_NAME = "applicationSetting.json";
        public const string APPLICATION_SETTINGS_PREFERENCES_KEY = "ApplicationSettings_v1_0_0";
        public const string BACKUP_SETTING_FILE_NAME = "backupSettings.json";
        public const string SYNCHRONIZE_UPLOAD_BACKUPER_SETTING_NAME = "synchronize_upload";
        public const string SYNCHRONIZE_DOWNLOAD_BACKUPER_SETTING_NAME = "synchronize_download";
        public const string BEFORE_UPDATE_BACKUPER_SETTING_NAME = "backupBeforeSave";

        public static string BackupSettingFullPath { 
            get { return Path.Combine(FileSystem.Current.AppDataDirectory, BACKUP_SETTING_FILE_NAME); } }
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();
            
            
            string fileInfosFileFullPath = Path.Combine(FileSystem.Current.AppDataDirectory, FILE_INFOS_FILE_NAME);

            //var backuperSettings = InitializeSettings(builder.Services);
            IApplicationSettingsManagment applicationSettingsManagment = new ApplicationSettingsManagment();
            applicationSettingsManagment.Restore();
            builder.Services.AddSingleton(applicationSettingsManagment);

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            if (!BackuperMegaHelper.CheckAssembly(assemblies))
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            var backuperCoreBuildOption = new Backuper_Core.Models.BackuperCoreDIOptionModel()
            {
                backupSettings = applicationSettingsManagment.ApplicationSettings.BackupSettings,
                fileInfosFullPath = fileInfosFileFullPath,
                logger = new MockLogger(),
                //dictionaryFileBuilders = dictionaryFileBuilders,
                //dictionaryDirectoryBuilders = dictionaryDirectoryBuilders
                assemblies = assemblies
            };
            
            builder.Services.RegisterBackuperCore(backuperCoreBuildOption);
            builder.Services.AddScoped<IBackuperWrapperService, BackuperWrapperService>();
            builder.Services.AddScoped<ICryptService, CryptService_Android>();
            builder.Services.AddScoped<IDataStore<Item>, BlockStoreService>();
            builder.Services.AddScoped<IDataStore<Group>, GroupStoreService>();
            

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
        private static CBackupSettings InitializeSettings(IServiceCollection services)
        {
            #region oldSetting
            AppSettings appSettings = new AppSettings()
            {
                DirCryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
                    {
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "Crypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, MAIN_CRYPT_FILE_NAME),
                    },
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "TestCrypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestCrypt")
                    }
                }),
                selected_crypr_file = "Crypt",
                DirDecryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
                    {
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "DecryptedCrypt",    
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestCrypt")
                    }
                }),
                selected_decrypr_file = "DecryptedCrypt"
            };
            //services.AddSingleton<IAppSettings>(appSettings);
            SearchSettings searchSettings = new SearchSettings()
            {
                CaseSensitive = false,
                SearchEverywhere = false,
                SearchInHeader = false,
                SearchInTegs = false,
                SearchUntilFirstMatch = false,
                ViewServiceInformation = false
            };
            //services.AddSingleton<ISearchSettings>(searchSettings);

            Dictionary<string, string> backupsPath_syncronize = new();//to
            backupsPath_syncronize.Add("0", $"Synchronize/{MAIN_CRYPT_FILE_NAME}");
            Dictionary<string, string> backupsPath_beforeUpdate = new();//to
            backupsPath_beforeUpdate.Add("0", "AppBackups/" + Path.GetFileName(appSettings.SelectedCryptFile.Path) + "{{dateTime%&%Format:yyyy-MM-dd_hh-mm}}");
            Dictionary<string, string> savedFilePath_synchronize = new();//from
            savedFilePath_synchronize.Add("0", appSettings.SelectedCryptFile.Path);
            Dictionary<string, string> savedFilePath_beforeUpdate = new();//from
            savedFilePath_beforeUpdate.Add("0", appSettings.SelectedCryptFile.Path);

            var backuperSettings = new CBackupSettings(new BackupSetting[]
                    {
                    new BackupSetting()
                    {
                        Name = SYNCHRONIZE_UPLOAD_BACKUPER_SETTING_NAME,
                        ProviderName_From = "Mega",
                        ProviderName_To = "LocalStorage",
                        WhatDoWithFile = WhatDoWithFile.CopyIfNever,
                        RetainedFileCountLimit = 1,
                        SavedFilePaths = savedFilePath_synchronize,//to
                        BackupPaths = backupsPath_syncronize,//form
                        Credentials = new string[] {
                            ""
                        },
                        Cron = "* * * * *"
                    },
                    new BackupSetting()
                    {
                        Name = SYNCHRONIZE_DOWNLOAD_BACKUPER_SETTING_NAME,
                        ProviderName_From = "LocalStorage",
                        ProviderName_To = "Mega",
                        WhatDoWithFile = WhatDoWithFile.CopyIfNever,
                        RetainedFileCountLimit = 1,
                        SavedFilePaths = backupsPath_syncronize,//to
                        BackupPaths = savedFilePath_synchronize,//from
                        Credentials = new string[] {
                            ""
                        },
                        Cron = "* * * * *"
                    },
                    new BackupSetting()
                    {
                        Name = BEFORE_UPDATE_BACKUPER_SETTING_NAME,
                        ProviderName_From = "LocalStorage",
                        ProviderName_To = "Mega",
                        WhatDoWithFile = WhatDoWithFile.CopyIfNever,
                        RetainedFileCountLimit = 1,
                        SavedFilePaths = savedFilePath_beforeUpdate,//to
                        BackupPaths = backupsPath_beforeUpdate,//from
                        Credentials = new string[] {
                            ""
                        },
                        Cron = "* * * * *"
                    }
            });
            #endregion
            #region newSettings
            ApplicationSettings applicationSettings = null;
            //bool applicationSettingsExists = Preferences.Default.ContainsKey(applicationSettingsPreferencesKey);
            string jsonContent = Preferences.Default.Get<string>(APPLICATION_SETTINGS_PREFERENCES_KEY, new ApplicationSettings()
            {
                AppSettings = appSettings,
                BackupSettings = backuperSettings,
                SearchSettings = searchSettings
            }.Serialize());
            applicationSettings = JsonConvert.DeserializeObject<ApplicationSettings>(jsonContent);
            services.AddSingleton(applicationSettings);
            
            #endregion
            return backuperSettings;
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