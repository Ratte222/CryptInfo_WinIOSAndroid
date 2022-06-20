using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

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
}