using CryptApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CryptApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }
        public string PasswordFromEntry { get; set; }
        public bool CB_TestMode { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            
        }

        private async void OnLoginClicked(object obj)
        {
        
        //https://question-it.com/questions/2850046/kak-peredavat-parametry-s-odnoj-stranitsy-na-druguju-v-xamarin-forms-i-kak-svjazat-predstavlenie-stranitsu-i-ego-viewmodel-i-initsializirovat-obekt-viewmodel
            CryptApp.Helpers.AppSettings appSettings = DependencyService.Get
                <CryptApp.Helpers.AppSettings>(DependencyFetchTarget.GlobalInstance);
            appSettings.Password = PasswordFromEntry;
            appSettings.TestMode = CB_TestMode;
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
        }
    }
}
