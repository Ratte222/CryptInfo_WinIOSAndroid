using Backuper_Core.Services;
using CommonForCryptPasswordLibrary.Interfaces;
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
        IAppSettings _appSettings;
        public Command ExecuteReadCommand { get; }
        public Command ExecuteWriteCommand { get; }
        public Command ExecuteSynchronizeWithAWS { get; }

        public FileViewModel()
        {
            _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IAppSettings>();
            ExecuteReadCommand = new Command(ReadFromFile);
            ExecuteWriteCommand = new Command(WriteToFile);
            ExecuteSynchronizeWithAWS = new Command(SynchronizeWithAWS);
        }

        private void ReadFromFile()
        {
            var result = File.ReadAllText(_appSettings.SelectedCryptFile.Path);
            StateText = "Readed";
            FileContent = result;
        }
        private void WriteToFile()
        {
            File.WriteAllText(_appSettings.SelectedCryptFile.Path, FileContent);
            StateText = "Saved";
        }

        private void SynchronizeWithAWS()
        {
            
        }
    }
}
