using AutoMapper;
using CommonForCryptPasswordLibrary.Helpers;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Services;
using ConsoleCrypt.AutoMapper;
using ConsoleCrypt.Contracts;
using ConsoleCrypt.Helpers;
using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

namespace ConsoleCrypt
{
    //https://andrewlock.net/using-dependency-injection-in-a-net-core-console-application/
    //https://pradeeploganathan.com/dotnet/dependency-injection-in-net-core-console-application/
    //https://metanit.com/sharp/aspnet5/6.2.php
    class Startup
    {
        public void ConfigureService(ref ServiceProvider services)
        {
            //setup our DI
            var serviceCollection = new ServiceCollection()
                //.AddLogging()
                //.AddSingleton<ImyIO_Console, MyIO_Console>()
                //.AddSingleton<I_InputOutput, InputOutputFile>()
                .AddAutoMapper(typeof(AutoMapperProfile));
            
            serviceCollection.AddSingleton<IAppSettingsConsole>(provider => {
                AppSettings appSettings = new AppSettings();
                return appSettings.DeserializeFromFile(appSettings.PathToSettings);
            });
            serviceCollection.AddSingleton<ISearchSettings>(provider =>
            {
                SearchSettings searchSettings = new SearchSettings();
                return searchSettings.DeserializeFromFile(searchSettings.pathToSettings);
            });
            serviceCollection.AddSingleton<IAppSettings>(provider => provider.GetRequiredService<IAppSettingsConsole>());
            serviceCollection.AddScoped<ImyIO_Console, MyIO_Console>();
            serviceCollection.AddSingleton<CommandInterpreter>();

            serviceCollection.RegisterCryptographyPasswordLibrary(new CommonForCryptPasswordLibrary.Model.RegisterCryptographyPasswordLibraryOptions() { IMyIO_ImplementationType = typeof(MyIO_Console) });
            
            services = serviceCollection.BuildServiceProvider();
            
        }
    }
}
