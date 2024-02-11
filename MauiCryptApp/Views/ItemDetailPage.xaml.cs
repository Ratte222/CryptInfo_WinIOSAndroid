using MauiCryptApp.Interfaces;
using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class ItemDetailPage : ContentPage
{
    
	ItemDetailViewModel itemDetailViewModel;
    public ItemDetailPage()
	{
		InitializeComponent();
        
        BindingContext = itemDetailViewModel = new ItemDetailViewModel();
		itemDetailViewModel.DisplayAlert += DisplayAlert;
		itemDetailViewModel.DisplayAlert_ += DisplayAlert;
	}

	//protected override bool OnBackButtonPressed()
	//{
 //       var navigationStack = Navigation.NavigationStack;

	//	//return base.OnBackButtonPressed();
	//	//Shell.Current.Navigation.PopAsync();
	//	Shell.Current.GoToAsync("..");
	//	return true;
 //   }

	async void OnPasswordLabelTapped(object sender, EventArgs e)
	{
		await Clipboard.SetTextAsync(PasswordEntry.Text);
		await DisplayAlert("Info", "Password copied to clipboard", "Ok");

	}

    async void OnEmailLabelTapped(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(EmailEntry.Text);
        await DisplayAlert("Info", "Email copied to clipboard", "Ok");
		
    }

	async void OnUpdate(object sender, EventArgs e)
	{
		
	}
}