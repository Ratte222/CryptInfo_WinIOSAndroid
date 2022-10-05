using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.ViewModels;

namespace MauiCryptApp.Views;

public partial class FilePage : ContentPage
{
    IAppSettings _appSettings;
    
    public FilePage()
	{
		InitializeComponent();
        //BindingContext = new FileViewModel();
        _appSettings = DependencyService.Get<IAppSettings>();
	}

    public void OnReadFromFileClicked(object sender, EventArgs e)
    {
        var result = File.ReadAllText(_appSettings.SelectedCryptFile.Path);
        StateLabel.Text = "Readed";
        Input.Text = result;
    }
    public void OnWriteToFileClicked(object sender, EventArgs e)
    {
        File.WriteAllText(_appSettings.SelectedCryptFile.Path, Input.Text);
        StateLabel.Text = "Saved";
    }
}