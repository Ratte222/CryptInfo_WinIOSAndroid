using Backuper_Core.Configurations;
using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.ViewModels
{
    public class LoginSynchronizeViewModel
    {
        public Command LoginCommand { get; }
        public string PasswordFromEntry { get; set; }
        public LoginSynchronizeViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }
        /// <summary>
        /// i am don`t know why variant from LoginPage doesn`t work. So I use another way. For this case route registred in AppShall.cs file
        /// </summary>
        /// <param name="obj"></param>
        private async void OnLoginClicked(object obj)
        {
            //string route = $"//{nameof(SynchronizePage)}";//?{nameof(SynchronizeViewModel.CryptKey)}={PasswordFromEntry}";
            //Routing.RegisterRoute(route, typeof(SynchronizePage));
            //string route = $"///{nameof(ItemsPage)}?{nameof(ItemsViewModel.Password)}={PasswordFromEntry}";
            //Routing.RegisterRoute(route, typeof(ItemsPage));
            var data = new Dictionary<string, object>();
            data.Add(nameof(SynchronizeViewModel.CryptKey), PasswordFromEntry);
            await Shell.Current.GoToAsync("synchronize", data);
        }

        
    }
}
