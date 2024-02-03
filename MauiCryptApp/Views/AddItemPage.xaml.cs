using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class AddItemPage : ContentPage
{
	private AddItemViewModel _addItemViewModel;
	public AddItemPage()
	{
		InitializeComponent();
		BindingContext = _addItemViewModel = new AddItemViewModel();
		_addItemViewModel.DisplayAlert += DisplayAlert;
	}

	public async Task DisplayAlert(string title, string body)
	{
		await DisplayAlert(title, body, "Ok");
	}
}