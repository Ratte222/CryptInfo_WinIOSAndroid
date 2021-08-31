using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Services;
using ConsoleCrypt.AutoMapper;
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
            serviceCollection.AddScoped<ImyIO_Console, MyIO_Console>();
            serviceCollection.AddSingleton<IAppSettings>(provider => {
                AppSettings appSettings = new AppSettings();
                return appSettings.DeserializeFromFile(appSettings.pathToSettings);
            });
            serviceCollection.AddSingleton<ICryptDecrypt, CryptDecrypt>();
            serviceCollection.AddSingleton<ISearchSettings>(provider =>
            {
                SearchSettings searchSettings = new SearchSettings();
                return searchSettings.DeserializeFromFile(searchSettings.pathToSettings);
            });
            serviceCollection.AddSingleton<ICryptBlock>(provider =>
            {
                return new CryptBlockService(provider.GetService<ICryptDecrypt>());
            });
            serviceCollection.AddSingleton<ICryptGroup>(provider =>
            {
                return new CryptGroupService(provider.GetService<ICryptDecrypt>());
            });
            serviceCollection.AddSingleton<I_InputOutput>(provider =>
            {
                return new InputOutputFile(provider.GetService<ImyIO_Console>(),
                    provider.GetService<IAppSettings>(), provider.GetService<ISearchSettings>());
            });
            serviceCollection.AddSingleton<CommandInterpreter>(provider =>
            {
                return new CommandInterpreter(provider.GetService<I_InputOutput>(),
                    provider.GetService<ImyIO_Console>(), provider.GetService<IAppSettings>(),
                    provider.GetService<ISearchSettings>(), provider.GetService<IMapper>(),
                    provider.GetService<ICryptBlock>(), provider.GetService<ICryptGroup>());
            });
            services = serviceCollection.BuildServiceProvider();
            //configure console logging
            //serviceProvider
            //    .GetService<ILoggerFactory>()
            //    .AddConsole(LogLevel.Debug);

            //var logger = serviceProvider.GetService<ILoggerFactory>()
            //    .CreateLogger<Program>();
            //logger.LogDebug("Starting application");

            //do the actual work here
            //var bar = serviceProvider.GetService<IBarService>();
            //bar.DoSomeRealWork();

            //logger.LogDebug("All done!");

        }
    }
}
