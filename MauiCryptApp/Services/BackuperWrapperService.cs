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
        public List<LogModel> LogsStorage { get; private set; } = new();
        public BackuperWrapperService(ApplicationSettings applicationSettings, 
            BackupJob backupJob)
        {
            _applicationSettings = applicationSettings;
            _backupJob = backupJob;

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
        }

        public async Task MakeBackupBeforeUpdate()
        {
            await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x => x.Name == "backupBeforeSave"));
        }

        public async Task Synchronize()
        {
            await MakeBackup(_applicationSettings.BackupSettings.BackupSettings.First(x => x.Name == "synchronize"));
        }
    }
}
