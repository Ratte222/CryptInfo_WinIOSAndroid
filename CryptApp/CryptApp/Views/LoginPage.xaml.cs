using CryptApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        private void CB_ViewPassword_CheckedChange(object sender, CheckedChangedEventArgs e)
        {
            PasswordEntry.IsPassword = !e.Value;
        }

        //private void BT_Login_Click(object sender, EventArgs e)
        //{
        //    LoginViewModel loginViewModel = new LoginViewModel();
        //    loginViewModel.PasswordFromEntry = PasswordEntry.Text;
        //}
    }
}