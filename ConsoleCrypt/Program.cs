using System;
using System.IO;
using System.Xml.Serialization;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using ConsoleCrypt.Helpers;
using ConsoleCrypt.WorkWithJson;

namespace ConsoleCrypt
{
    class Program
    {
        static I_InputOutput _inputOutputFile;
        static ImyIO_Console _console_IO = new MyIO_Console();
        static ISettings _settings;
        static void Main(string[] args)
        {
            AppSettings appSettings = new AppSettings();
            appSettings = appSettings.Deserialize(appSettings.pathToSettings);
            SearchSettings searchSettings = new SearchSettings();
            searchSettings = searchSettings.Deserialize(searchSettings.pathToSettings);
            _settings = new Settings(appSettings, searchSettings, _console_IO);            
            _inputOutputFile = new InputOutputFile(_console_IO, _settings);            
            CommandInterpreter consoleInterpreter = new CommandInterpreter(_inputOutputFile, _console_IO, _settings);
            if(!System.IO.File.Exists(_settings.GetDirDecryptFile()))
            {
                CryptFileModel cryptFileModel = new CryptFileModel();
                cryptFileModel.DecryptInfoContent = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<CryptBlockModel>>();
                cryptFileModel.DecryptInfoContent.Add("SocialWide", new System.Collections.Generic.List<CryptBlockModel>(new[] {
                new CryptBlockModel(){Id = 0, Title = "Picabu", Email="Artur@gmail.com", Password="12345678", UserName="Artur321" },
                new CryptBlockModel(){Id = 0, Title = "Instagram", Email="Artur@gmail.com", Password="12345678", UserName="Artur321" }
                }));
                cryptFileModel.DecryptInfoContent.Add("Work", new System.Collections.Generic.List<CryptBlockModel>(new[] {
                new CryptBlockModel(){Id = 0, Title = "Google", Email="Artur@gmail.com", Password="12345678", UserName="Artur321" },
                new CryptBlockModel(){Id = 0, Title = "LincedIn", Email="Artur@gmail.com", Password="12345678", UserName="Artur321" }
                }));
                SerializeDeserializeJson<CryptFileModel> serializeDeserializeJson = new SerializeDeserializeJson<CryptFileModel>();
                serializeDeserializeJson.Serialize(cryptFileModel, _settings.GetDirDecryptFile());
            }
            if(args.Length > 0)
            {
                if (args[0].ToLower() == "loop")
                {
                    consoleInterpreter.Start();
                }
                else
                {
                    consoleInterpreter.InterpretCommand(args);
                }
            }            
        }       
    }
}
