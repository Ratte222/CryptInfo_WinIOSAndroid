using MauiCryptApp.Helpers;
using MauiCryptApp.Services;

namespace MauiCryptApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.RegisterSingleton(new AppSettings());
            DependencyService.Register<DataStoreService>();
            MainPage = new AppShell();
        }
    }
}