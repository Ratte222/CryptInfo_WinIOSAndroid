using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary
{
    public interface IMyIO
    {
        string ReadLine();
        void WriteLine(string content);
        void WriteLineUnknownCommand(string command);
        void WriteLineTooFewParameters();
        string GetHiddenInput();
        void HandleMessage(string _msg, Exception ex);

    }
}
