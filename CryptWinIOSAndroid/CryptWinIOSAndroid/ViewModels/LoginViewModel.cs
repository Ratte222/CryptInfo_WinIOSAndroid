using CryptWinIOSAndroid.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CryptWinIOSAndroid.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        
        private string key;
        private string importantKey;
        
        public LoginViewModel()
        {
            //LoginCommand = new Command(OnLoginClicked, ValidateSave);
            LoginCommand = new Command(OnLoginClicked);
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(key)
                && !String.IsNullOrWhiteSpace(importantKey);
        }

        public string Key
        {
            get => key;
            set => SetProperty(ref key, value);
        }

        public string ImportantKey
        {
            get => importantKey;
            set => SetProperty(ref importantKey, value);
        }
        private async void OnLoginClicked(object obj)
        {
            bool temp = await DataStore.SetKeys(Key, ImportantKey);
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        }
    }
}
