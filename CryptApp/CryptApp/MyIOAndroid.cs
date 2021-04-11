using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary;
namespace CryptApp
{
    public class MyIOAndroid:MyIO
    {
        public string Output;
        public string Input = "";
        public override string ReadLine()
        {
            //return base.ReadLine();
            return Input;
        }
        public override void HandleMessage(string _msg, Exception ex)
        {
            //base.HandleMessage(_msg, ex);
        }
        public override void WriteLine(string content)
        {
            //base.WriteLine(content);
            Output += $"{content}\r\n";
        }
    }
}
