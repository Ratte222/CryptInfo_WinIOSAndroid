using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;

namespace CryptApp
{
    public class MyIOAndroid:IMyIO
    {
        public string Output;
        public string Input = "";
        public string ReadLine()
        {
            //return base.ReadLine();
            return Input;
        }
        public void HandleMessage(string _msg, Exception ex)
        {
            //base.HandleMessage(_msg, ex);
        }
        public void WriteLine(string content)
        {
            //base.WriteLine(content);
            Output += $"{content}\r\n";
        }
    }
}
