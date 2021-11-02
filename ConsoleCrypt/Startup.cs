using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
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
            serviceCollection.AddScoped<ImyIO_Console, MyIO_Console>();
            serviceCollection.AddSingleton<IAppSettingsConsole>(provider => {
                AppSettings appSettings = new AppSettings();
                return appSettings.DeserializeFromFile(appSettings.PathToSettings);
            });
            serviceCollection.AddSingleton<IEncryptDecryptService, EncryptDecryptService>();
            serviceCollection.AddSingleton<ISearchSettings>(provider =>
            {
                SearchSettings searchSettings = new SearchSettings();
                return searchSettings.DeserializeFromFile(searchSettings.pathToSettings);
            });
            serviceCollection.AddSingleton<IBlockService>(provider =>
            {
                return new BlockService(provider.GetService<IEncryptDecryptService>());
            });
            serviceCollection.AddSingleton<IGroupService>(provider =>
            {
                return new GroupService(provider.GetService<IEncryptDecryptService>());
            });
            serviceCollection.AddSingleton<IMainLogicService>(provider =>
            {
                return new MainLogicService(provider.GetService<ImyIO_Console>(),
                    provider.GetService<IAppSettingsConsole>(), provider.GetService<ISearchSettings>(),
                    provider.GetService<IGroupService>(), provider.GetService<IBlockService>());
            });
            serviceCollection.AddSingleton<CommandInterpreter>(provider =>
            {
                return new CommandInterpreter(provider.GetService<IMainLogicService>(),
                    provider.GetService<ImyIO_Console>(), provider.GetService<IAppSettingsConsole>(),
                    provider.GetService<ISearchSettings>(), provider.GetService<IMapper>(),
                    provider.GetService<IBlockService>(), provider.GetService<IGroupService>(),
                    provider.GetService<IEncryptDecryptService>());
            });
            //serviceCollection.AddSingleton<FluentCommandLineParser<>>
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
