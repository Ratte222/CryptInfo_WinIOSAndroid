using System;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleCrypt
{
    class Program
    {
        static C_InputOutputFile InputOutputFile = new C_InputOutputFile();
        static void Main(string[] args)
        {
            Init();
            ConsoleInterpreter consoleInterpreter = new ConsoleInterpreter(InputOutputFile);
            consoleInterpreter.Start();
            //Console.WriteLine("Hello World!");
        }
        
        protected static void Init()
        {
            InputOutputFile.LoadSetting();
        }
    }
}
