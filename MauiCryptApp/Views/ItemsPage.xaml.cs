using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class ItemsPage : ContentPage
{
	ItemsViewModel itemsViewModel;
	public ItemsPage()
	{
		InitializeComponent();
		BindingContext = itemsViewModel = new ItemsViewModel();
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		itemsViewModel.OnAppearing();
	}

	protected override bool OnBackButtonPressed()
	{
		return base.OnBackButtonPressed();
	}
}