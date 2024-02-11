using AuxiliaryLib.Extensions;
using Backuper_Core.Configurations;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    internal class ApplicationSettingsManagment : IApplicationSettingsManagment
    {
        private ApplicationSettings _applicationSettings;
        public ApplicationSettings ApplicationSettings { get { return _applicationSettings; } }
        private string _applicationSettingsFullPath => Path.Combine(FileSystem.Current.AppDataDirectory, MauiProgram.APPLICATION_SETTINGS_FILE_NAME);
        public void Restore()
        {
            if(!File.Exists(_applicationSettingsFullPath))
            {
                SetDefaultApplicationSettings();
                Save();
            }
            else
            {
                _applicationSettings = WorkWirhJsonExtension.DeserializeFromFile<ApplicationSettings>(_applicationSettingsFullPath);
            }
        }

        private void SetDefaultApplicationSettings()
        {
            
            AppSettings appSettings = new AppSettings()
            {
                DirCryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
                    {
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "Crypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, MauiProgram.MAIN_CRYPT_FILE_NAME),
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
            backupsPath_syncronize.Add("0", $"Synchronize/{MauiProgram.MAIN_CRYPT_FILE_NAME}");
            Dictionary<string, string> backupsPath_beforeUpdate = new();//to
            backupsPath_beforeUpdate.Add("0", appSettings.SelectedCryptFile.Path);
            Dictionary<string, string> savedFilePath_synchronize = new();//from
            savedFilePath_synchronize.Add("0", appSettings.SelectedCryptFile.Path);
            Dictionary<string, string> savedFilePath_beforeUpdate = new();//from
            savedFilePath_beforeUpdate.Add("0", "AppBackups/" + Path.GetFileName(appSettings.SelectedCryptFile.Path) + "{{dateTime%&%Format:yyyy-MM-dd_hh-mm}}");

            var backuperSettings = new CBackupSettings(new BackupSetting[]
                    {
                    new BackupSetting()
                    {
                        Name = MauiProgram.SYNCHRONIZE_DOWNLOAD_BACKUPER_SETTING_NAME,
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
                        Name = MauiProgram.SYNCHRONIZE_UPLOAD_BACKUPER_SETTING_NAME,
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
                        Name = MauiProgram.BEFORE_UPDATE_BACKUPER_SETTING_NAME,
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
            _applicationSettings = new ApplicationSettings()
            {
                AppSettings = appSettings,
                BackupSettings = backuperSettings,
                SearchSettings = searchSettings
            };
        }

        public void Save()
        {
            _applicationSettings.SerializeToFile(_applicationSettingsFullPath);
        }
    }
}
