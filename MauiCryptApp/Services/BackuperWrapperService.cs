using Backuper_Core.Configurations;
using Backuper_Core.Services;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    internal class BackuperWrapperService : IBackuperWrapperService
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly BackupJob _backupJob;
        private readonly MockLoggerForSynchronize _loggerMock;
        private readonly FileInfos _fileInfos;
        public List<LogModel> LogsStorage { get; private set; } = new();
        public string PrettyLogs { get => string.Join(Environment.NewLine, LogsStorage.Select(x=>x.Description).ToArray()); }
        public BackuperWrapperService(IApplicationSettingsManagment applicationSettingsManagement, 
            BackupJob backupJob,
            FileInfos fileInfos)
        {
            _applicationSettings = applicationSettingsManagement.ApplicationSettings;
            _backupJob = backupJob;
            _fileInfos = fileInfos;

            var bacokuperJobLogger = _backupJob.GetType().GetField("_logger", BindingFlags.NonPublic | BindingFlags.Instance);
            _loggerMock = new MockLoggerForSynchronize(LogsStorage);
            bacokuperJobLogger.SetValue(_backupJob, _loggerMock);            
        }

        public async Task MakeBackup(string backupName)
        {
            await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x=>x.Name == backupName));
        }

        public async Task MakeBackup(BackupSetting backupSetting)
        {
            await _backupJob.CreateBackupAsync(backupSetting);
            //ToDo: fix this crutch!
            _fileInfos.FileInfosData = new List<Backuper_Core.Configurations.FileInfo>();
            _fileInfos.Save();
        }

        public async Task MakeBackupBeforeUpdate()
        {
            if(_applicationSettings.CreateBackupBeforeUpdateOrCreateItem)
                await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x => x.Name == MauiProgram.BEFORE_UPDATE_BACKUPER_SETTING_NAME));
        }

        public async Task Synchronize_Upload()
        {
            if(_applicationSettings.SyncAfterUpdateCreateItem)
                await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x => x.Name == MauiProgram.SYNCHRONIZE_UPLOAD_BACKUPER_SETTING_NAME));
        }
        public async Task Synchronize_Download()
        {
            if(_applicationSettings.SyncBeforeDecryptFile)
                await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x => x.Name == MauiProgram.SYNCHRONIZE_DOWNLOAD_BACKUPER_SETTING_NAME));
        }

        public void CleanLogs()
        {
            LogsStorage.Clear();
        }
    }
}
