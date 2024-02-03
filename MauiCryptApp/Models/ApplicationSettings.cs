using Backuper_Core.Configurations;
using MauiCryptApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Models
{
    public class ApplicationSettings
    {
        public CBackupSettings BackupSettings { get;set; }
        public SearchSettings SearchSettings { get;set; }
        /// <summary>
        /// Crypt module settings
        /// </summary>
        public AppSettings AppSettings { get;set; }

    }
}
