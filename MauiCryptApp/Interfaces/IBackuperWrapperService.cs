using Backuper_Core.Configurations;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Interfaces
{
    interface IBackuperWrapperService
    {
        public Task Synchronize_Upload();
        public Task Synchronize_Download();
        public Task MakeBackupBeforeUpdate();
        public Task MakeBackup(string backupName);
        public Task MakeBackup(BackupSetting backupSetting);
        public List<LogModel> LogsStorage { get; }
    }
}
