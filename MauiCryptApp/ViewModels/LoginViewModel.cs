using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using MauiCryptApp.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    public class LoginViewModel:BaseViewModel
    {
        public Command LoginCommand { get; }
        public string PasswordFromEntry { get; set; }
        public bool CB_TestMode { get; set; }

        public delegate void ClearPasswordField();
        public event ClearPasswordField OnClearPasswordField;

        private string encryptedFileName;
        public string EncryptedFileName { get { return encryptedFileName; } set { SetProperty(ref encryptedFileName, value); } }
        private readonly ApplicationSettings _settings;
        private readonly IBackuperWrapperService _backuper;
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            _settings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IApplicationSettingsManagment>().ApplicationSettings;           
            _backuper = MauiProgram.ServiceScope.ServiceProvider.GetService<IBackuperWrapperService>();
        }
        private async void OnLoginClicked(object obj)
        {
            await _backuper.Synchronize_Download();
            string password = (string)PasswordFromEntry.Clone();
            //PasswordFromEntry = string.Empty;
            OnClearPasswordField.Invoke();
            await Shell.Current.GoToAsync($"/{nameof(ItemsPage)}?{nameof(ItemsViewModel.Password)}={password}");
        }

        public void AppearingHandler(object sender, EventArgs args)
        {
            EncryptedFileName = $"Encrypted preset: {_settings.AppSettings.SelectedCryptFile.Name}; path: {_settings.AppSettings.SelectedCryptFile.Path}";
        }
    }
}
