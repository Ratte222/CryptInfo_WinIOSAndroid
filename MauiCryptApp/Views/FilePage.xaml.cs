using Backuper_Core.Configurations;
using Backuper_Core.Services;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using Microsoft.Extensions.Logging;

namespace MauiCryptApp.Views;

public partial class FilePage : ContentPage
{
    private readonly ApplicationSettings _appSettings;
    private readonly BackupJob _backupJob;
    private readonly IBackupSettings _backupSettings;
    private readonly Microsoft.Extensions.Logging.ILogger<FilePage> _logger;
    public FilePage()
	{
		InitializeComponent();
        //BindingContext = new FileViewModel();
        //_appSettings = DependencyService.Get<IAppSettings>();
        _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IApplicationSettingsManagment>().ApplicationSettings;
        _backupJob = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<BackupJob>();
        //_backupJob = backupJob;//DependencyService.Get<BackupJob>(DependencyFetchTarget.NewInstance);
        _backupSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IBackupSettings>();//DependencyService.Get<IBackupSettings>(DependencyFetchTarget.GlobalInstance);
        _logger = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<FilePage>>();
       
	}
    public void OnReadFromFileClicked(object sender, EventArgs e)
    {
        var result = File.ReadAllText(_appSettings.AppSettings.SelectedCryptFile.Path);
        StateLabel.Text = "Readed";
        Input.Text = result;
    }
    public void OnWriteToFileClicked(object sender, EventArgs e)
    {
        File.WriteAllText(_appSettings.AppSettings.SelectedCryptFile.Path, Input.Text);
        StateLabel.Text = "Saved";
    }

    public async void OnSynchronizeWithAWSClicked(object sender, EventArgs e)
    {
        await _backupJob.CreateBackupAsync(_backupSettings.BackupSettings.First());
        _logger.LogWarning("Synchronize");
    }
}