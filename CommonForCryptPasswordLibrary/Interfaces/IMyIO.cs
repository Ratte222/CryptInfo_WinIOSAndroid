using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IMyIO
    {
        string ReadLine();
        void WriteLine(string content);
        void HandleMessage(string _msg, Exception ex);

    }
}
