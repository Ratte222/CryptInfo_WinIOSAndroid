using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
namespace ConsoleCrypt.Commands
{
    [Verb("encrypt", HelpText = "Encrypt something")]
    public class EncryptCommand
    {
        [Option('f', "file", Required = true, HelpText = "Encrypt selected file", Default = false)]
        public bool File { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
