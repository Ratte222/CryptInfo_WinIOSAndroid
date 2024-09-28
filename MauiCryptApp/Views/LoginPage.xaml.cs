using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _loginViewModel;
	public LoginPage()
	{
		InitializeComponent();
        this.BindingContext = _loginViewModel = new LoginViewModel();
        this.Appearing += _loginViewModel.AppearingHandler;
        _loginViewModel.OnClearPasswordField += OnClearPasswordField;
    }
    private void CB_ViewPassword_CheckedChange(object sender, CheckedChangedEventArgs e)
    {
        PasswordEntry.IsPassword = !e.Value;
    }

    private void OnClearPasswordField()
    {
        PasswordEntry.Text = string.Empty;
    }
}