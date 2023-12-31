using Backuper_Core.Configurations;
using Backuper_Core.Services;
using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MauiCryptApp.Views;

public partial class FilePage : ContentPage
{
    private readonly IAppSettings _appSettings;
    private readonly BackupJob _backupJob;
    private readonly IBackupSettings _backupSettings;
    private readonly Microsoft.Extensions.Logging.ILogger<FilePage> _logger;
    public FilePage(/*BackupJob backupJob, IBackupSettings backupSettings*/)
	{
		InitializeComponent();
        //BindingContext = new FileViewModel();
        //_appSettings = DependencyService.Get<IAppSettings>();
        _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IAppSettings>();
        _backupJob = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<BackupJob>();
        //_backupJob = backupJob;//DependencyService.Get<BackupJob>(DependencyFetchTarget.NewInstance);
        _backupSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IBackupSettings>();//DependencyService.Get<IBackupSettings>(DependencyFetchTarget.GlobalInstance);
        _logger = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<FilePage>>();
        _logger.Log(LogLevel.Warning, "TestMessage");
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

    public async void OnSynchronizeWithAWSClicked(object sender, EventArgs e)
    {
        await _backupJob.CreateBackupAsync(_backupSettings.BackupSettings.First());
        _logger.LogWarning("Synchronize");
        //_logger.LogWarning(Backuper_Mega.Helpers.JustForLinkPackageToMainProject.FuckIt);
    }
}