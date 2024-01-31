using MauiCryptApp.Interfaces;
using MauiCryptApp.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace MauiCryptApp.Views;

public partial class ItemDetailPage : ContentPage
{
    private readonly IBackuperWrapperService _backuperWrapperService;
	ItemDetailViewModel itemDetailViewModel;
    public ItemDetailPage()
	{
		InitializeComponent();
        _backuperWrapperService = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IBackuperWrapperService>();
        BindingContext = itemDetailViewModel = new ItemDetailViewModel();
	}

	protected override bool OnBackButtonPressed()
	{
        var navigationStack = Navigation.NavigationStack;

		//return base.OnBackButtonPressed();
		//Shell.Current.Navigation.PopAsync();
		Shell.Current.GoToAsync("..");
		return true;
    }

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
		await _backuperWrapperService.MakeBackupBeforeUpdate();
		string logs = string.Join("\r\n", _backuperWrapperService.LogsStorage);
		var result = await DisplayAlert("Backup logs", logs, "ok", "cancel");
		if(result)//do updating of instance
		{
			await itemDetailViewModel.UpdateItem();
		}
	}
}