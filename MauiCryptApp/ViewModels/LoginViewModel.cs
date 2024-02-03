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
        private string encryptedFileName;
        public string EncryptedFileName { get { return encryptedFileName; } set { SetProperty(ref encryptedFileName, value); } }
        private readonly ApplicationSettings _settings;
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            _settings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ApplicationSettings>();           
        }
        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"/{nameof(ItemsPage)}?{nameof(ItemsViewModel.Password)}={PasswordFromEntry}");
        }

        public void AppearingHandler(object sender, EventArgs args)
        {
            EncryptedFileName = $"Encrypted preset: {_settings.AppSettings.SelectedCryptFile.Name}; path: {_settings.AppSettings.SelectedCryptFile.Path}";
        }
    }
}
