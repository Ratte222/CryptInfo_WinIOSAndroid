namespace MauiCryptApp.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
        BindingContext = new ViewModels.AboutViewModel();
    }
}