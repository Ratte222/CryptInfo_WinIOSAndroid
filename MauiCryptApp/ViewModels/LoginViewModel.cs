using MauiCryptApp.Views;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    public class LoginViewModel
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
            string route = $"///{nameof(ItemsPage)}?{nameof(ItemsViewModel.Password)}={PasswordFromEntry}";
            Routing.RegisterRoute(route, typeof(ItemsPage));
            await Shell.Current.GoToAsync(route);
        }
    }
}
