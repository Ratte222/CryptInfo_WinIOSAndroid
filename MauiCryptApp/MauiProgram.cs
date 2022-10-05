using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace MauiCryptApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<IAppSettings>(provider => {
                AppSettings appSettings = new AppSettings()
                {
                    DirCryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
                {
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "Crypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt"),
                    },
                    new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
                    {
                        Name = "TestCrypt",
                        Path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestCrypt")
                    }
                }),
                    selected_crypr_file = "Crypt"
                };
                return appSettings;
            });
            return builder.Build();
        }
    }
}