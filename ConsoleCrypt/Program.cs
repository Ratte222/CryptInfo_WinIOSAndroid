using System;
using System.IO;
using System.Xml.Serialization;
using CommonForCryptPasswordLibrary;
namespace ConsoleCrypt
{
    class Program
    {
        static C_InputOutputFile InputOutputFile;
        static MyIO_Console console_IO = new MyIO_Console();
        static Settings settings;
        static void Main(string[] args)
        {            
            settings = new Settings(console_IO);            
            InputOutputFile = new C_InputOutputFile(console_IO, settings);            
            CommandInterpreter consoleInterpreter = new CommandInterpreter(InputOutputFile, console_IO, settings);
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
