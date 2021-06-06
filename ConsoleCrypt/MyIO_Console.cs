using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary;
namespace ConsoleCrypt
{
    public interface ImyIO_Console: IMyIO
    {
        string ConsoleReadMultiline();
    }
    public class MyIO_Console:MyIO, ImyIO_Console
    {
        public string ConsoleReadMultiline()
        {
            Console.WriteLine("When you end - write \"end\". If you want to undo the input - write \"cancel\"");
            string line = "", result = "";
            do
            {
                //input code
                result += $"{line}\r\n";
                //Check for exit conditions
                line = $"{Console.ReadLine()}";
            } while (/*!String.IsNullOrWhiteSpace(line) && */!String.Equals(line.ToLower(), "end")
            && !String.Equals(line.ToLower(), "cancel"));
            if (String.Equals(line.ToLower(), "cancel"))
                return "";
            return result.TrimStart(new char[] { '\r', '\n' });
        }
    }
}
