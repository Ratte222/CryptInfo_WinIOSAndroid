using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class LoginSynchronizePage : ContentPage
{
	LoginSynchronizeViewModel LoginSynchronizeViewModel;
	public LoginSynchronizePage()
	{
		InitializeComponent();
		BindingContext = LoginSynchronizeViewModel = new LoginSynchronizeViewModel();
	}

    private void CB_ViewPassword_CheckedChange(object sender, CheckedChangedEventArgs e)
    {
        PasswordEntry.IsPassword = !e.Value;
    }
}