using System;
using System.Collections.Generic;
using System.Text;


namespace CommonForCryptPasswordLibrary
{
    public class MyIO:IMyIO
    {
        public virtual void WriteLine(string content)
        {
            Console.WriteLine(content);
        }
        public virtual void WriteLineUnknownCommand(string command)
        {
            Console.WriteLine($"->Unknown command. Use \"help {command}\" for give additional info");
        }
        public virtual void WriteLineTooFewParameters()
        {
            Console.WriteLine("->Too few parameters");
        }
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }
        public virtual string GetHiddenInput()
        {
            StringBuilder input = new StringBuilder();
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter) break;
                if (key.Key == ConsoleKey.Backspace && input.Length > 0) input.Remove(input.Length - 1, 1);
                else if (key.Key == ConsoleKey.Backspace && input.Length == 0) /*Console.Write("\a");*/Console.Beep();
                else if (key.Key != ConsoleKey.Backspace) input.Append(key.KeyChar);
            }
            return input.ToString();
        }
        public virtual void HandleMessage(string _msg, Exception ex)
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
            WriteLine(message);
#endif
        }
    }
}
