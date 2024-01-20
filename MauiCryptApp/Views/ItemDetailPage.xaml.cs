using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class ItemDetailPage : ContentPage
{
	public ItemDetailPage()
	{
		InitializeComponent();
		BindingContext = new ItemDetailViewModel();
	}

	protected override bool OnBackButtonPressed()
	{
        var navigationStack = Navigation.NavigationStack;

		//return base.OnBackButtonPressed();
		//Shell.Current.Navigation.PopAsync();
		Shell.Current.GoToAsync("..");
		return true;
    }
}