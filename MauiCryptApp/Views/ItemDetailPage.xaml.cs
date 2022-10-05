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
		return base.OnBackButtonPressed();
	}
}