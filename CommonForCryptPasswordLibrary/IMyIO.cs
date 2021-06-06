using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary
{
    public interface IMyIO
    {
        string ReadLine();
        void WriteLine(string content);
        string GetHiddenInput();
        void HandleMessage(string _msg, Exception ex);

    }
}
