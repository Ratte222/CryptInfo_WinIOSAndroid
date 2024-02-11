using Backuper_Core.Services;
using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{

    public class FileViewModel:BaseViewModel//does not use!!!
    {
        public string FileContent
        {
            get { return _fileContent; }
            set { _fileContent = value; }
        }
        string _fileContent;
        public string StateText;
        ApplicationSettings _appSettings;
        public Command ExecuteReadCommand { get; }
        public Command ExecuteWriteCommand { get; }
        public Command ExecuteSynchronizeWithAWS { get; }

        public FileViewModel()
        {
            _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IApplicationSettingsManagment>().ApplicationSettings;
            ExecuteReadCommand = new Command(ReadFromFile);
            ExecuteWriteCommand = new Command(WriteToFile);
            ExecuteSynchronizeWithAWS = new Command(SynchronizeWithAWS);
        }

        private void ReadFromFile()
        {
            var result = File.ReadAllText(_appSettings.AppSettings.SelectedCryptFile.Path);
            StateText = "Readed";
            FileContent = result;
        }
        private void WriteToFile()
        {
            File.WriteAllText(_appSettings.AppSettings.SelectedCryptFile.Path, FileContent);
            StateText = "Saved";
        }

        private void SynchronizeWithAWS()
        {
            
        }
    }
}
