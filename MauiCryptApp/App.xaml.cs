using CommonForCryptPasswordLibrary.Interfaces;
using MauiCryptApp.Helpers;
using MauiCryptApp.Services;

namespace MauiCryptApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DependencyService.RegisterSingleton(new SomeSettings());
            //DependencyService.RegisterSingleton<ISearchSettings>(new SearchSettings()
            //{
            //    CaseSensitive = false,
            //    SearchEverywhere = false,
            //    SearchInHeader = false,
            //    SearchInTegs = false,
            //    SearchUntilFirstMatch = false,
            //    ViewServiceInformation = false
            //});
            //AppSettings appSettings = new AppSettings()
            //{
            //    DirCryptFile = new List<CommonForCryptPasswordLibrary.Model.FileModelInSettings>(new[]
            //    {
            //        new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
            //        {
            //            Name = "Crypt",
            //            Path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt"),
            //        },
            //        new CommonForCryptPasswordLibrary.Model.FileModelInSettings()
            //        {
            //            Name = "TestCrypt",
            //            Path = Path.Combine(FileSystem.Current.AppDataDirectory, "TestCrypt")
            //        }
            //    }),
            //    selected_crypr_file = "Crypt"
            //};
            //DependencyService.RegisterSingleton<IAppSettings>(appSettings);
            DependencyService.Register<DataStoreService>();
            MainPage = new AppShell();
        }
    }
}