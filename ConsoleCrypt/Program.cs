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

        public static void HandleMessage(string _msg, Exception ex)
        {
            string message = "";
            if (ex != null)
            {
                message = $"{_msg} \r\n" +
                    $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                    $"Message = {ex?.Message?.ToString()} \r\n" +
                    $"Source = {ex?.Source?.ToString()} \r\n" +
                    $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                    $"TargetSite = {ex?.TargetSite?.ToString()}";
            }
            else
            {
               message = $"{_msg}";
            }
#if DEBUG
            InputOutputFile.ShowAPersone(message);
#endif
        }
    }
}
