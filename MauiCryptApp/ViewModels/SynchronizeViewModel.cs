using AuxiliaryLib.Extensions;
using Backuper_Core.Configurations;
using Backuper_Core.Services;
using CommonForCryptPasswordLibrary.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using MauiCryptApp.Helpers;
using MauiCryptApp.Models;
using MauiCryptApp.Views;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MauiCryptApp.ViewModels
{
    //https://www.youtube.com/watch?v=wMn1tuMfZ-0&ab_channel=AmichaiMantinband
    //[QueryProperty(nameof(CryptKey), nameof(CryptKey))]
    public partial class SynchronizeViewModel : ObservableObject, IQueryAttributable
    {
        //public List<LogModel> LogsStorage
        //{
        //    get { return logsStorage; }
        //    set { logsStorage = value; }
        //}
        //public List<StringModel> BackupSettingsJson
        //{
        //    get { return backupSettingsJson; }
        //    set { backupSettingsJson = value; }
        //}
        [ObservableProperty]
        public List<LogModel> logsStorage = new();
        [ObservableProperty]
        public List<StringModel> backupSettingsJson = new ();
        public string CryptKey { get; set; }
        private readonly IAppSettings _appSettings;
        private readonly BackupJob _backupJob;
        private FileInfos _fileInfos;
        public IBackupSettings BackupSettings
        {
            get
            {
                return _backupSettings;
            }
            set
            {
                BackupSettingsJson = value.BackupSettings.Serialize()
                .Split('\r')
                .Select(x => new StringModel() { Content = x }).ToList();
                _backupSettings = value;
            }
        }
        private IBackupSettings _backupSettings; 
        private readonly Microsoft.Extensions.Logging.ILogger<FilePage> _logger;
        private readonly ICryptService _cryptService;
        private readonly MockLoggerForSynchronize _loggerMock;
        public Command SynchronizeCommand { get; }
        public Command SaveBackupSettingsCommand { get; }
        public Command LoadBackupSettingsCommand { get; }
        public SynchronizeViewModel()
        {
            _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IAppSettings>();
            _fileInfos = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<FileInfos>();
            BackupSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IBackupSettings>();
            _cryptService = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ICryptService>();
            _logger = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<FilePage>>();
            //dances with logger substitution
            _backupJob = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<BackupJob>();
            var bacokuperJobLogger = _backupJob.GetType().GetField("_logger", BindingFlags.NonPublic | BindingFlags.Instance);
            _loggerMock = new MockLoggerForSynchronize(LogsStorage);
            bacokuperJobLogger.SetValue(_backupJob, _loggerMock);

            SynchronizeCommand = new Command(async () => await ExecuteSynchronizeCommand());
            SaveBackupSettingsCommand = new Command(() => ExecuteSaveBackupSettingsFileCommand());
            LoadBackupSettingsCommand = new Command(() => ExecuteLoadBackupSettingsFileCommand());
        }


        async Task ExecuteSynchronizeCommand()
        {
            try
            {
                await _backupJob.CreateBackupAsync(_backupSettings.BackupSettings.First());
            }
            catch(Exception ex)
            {
                Log("Create backup error",refreshLog:false);
                Log(ex.Serialize());
            }
            finally
            {
                PRefreshLogs();
            }
        }

        void ExecuteSaveBackupSettingsFileCommand()
        {
            try
            {

                string jsonContent = string.Join("\r ", backupSettingsJson.Select(x=>x.Content).ToArray());
                List<BackupSetting> listBackupSettings = jsonContent.Deserialize<List<BackupSetting>>();
                string cryptData = _cryptService.Encrypt(jsonContent, CryptKey);
                File.WriteAllText(MauiProgram.BackupSettingFullPath, cryptData);
                _fileInfos.FileInfosData = new List<Backuper_Core.Configurations.FileInfo>();
                _fileInfos.Save();//need for correct work synchonize
                CBackupSettings backupSettings = new CBackupSettings(listBackupSettings);
                BackupSettings = backupSettings;
                Log("backup settings successfully saved", refreshLog: false);
            }
            catch(Exception ex)
            {
                Log(ex.Serialize(), "Eroor", false);
            }
            finally
            {
                PRefreshLogs();
            }
        }

        void ExecuteLoadBackupSettingsFileCommand()
        {
            try
            {
                if (!File.Exists(MauiProgram.BackupSettingFullPath))
                {
                    ExecuteSaveBackupSettingsFileCommand();
                    Log("Create default backup setting file");
                }
                else
                {
                    string cryptData = File.ReadAllText(MauiProgram.BackupSettingFullPath);
                    string jsonContent = _cryptService.Decrypt(cryptData, CryptKey);

                    List<BackupSetting> listBackupSettings = jsonContent.Deserialize<List<BackupSetting>>();
                    CBackupSettings backupSettings = new CBackupSettings(listBackupSettings);
                    BackupSettings = backupSettings;
                }
                Log("Backup settings successfully loaded from crypt file");
            }
            catch (Exception ex)
            {
                Log(ex.Message, refreshLog: false);
                Log(ex.Serialize(), refreshLog: false);
            }
            finally { PRefreshLogs(); }
        }
        private void PRefreshLogs()
        {
            LogsStorage = new List<LogModel>(_loggerMock._logs.ToArray());
            //OnPropertyChanged("LogsStorage");

        }

        private void Log(string description, string level = null, bool refreshLog = true)
        {
            //LogsStorage.Add(new LogModel(description, level));
            _loggerMock.Log(LogLevel.Information, description);
            if(refreshLog)
                PRefreshLogs();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CryptKey = query[nameof(CryptKey)] as string;
            ExecuteLoadBackupSettingsFileCommand();
        }
    }

    public record StringModel
    {
        public string Content { get; set; }
    }
}
