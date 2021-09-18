using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("decrypt", HelpText = "Decrypt something")]
    public class DecryptCommand
    {
        [Option('f', "file", Required = true, HelpText = "Decrypt selected file", Default = false)]
        public bool File { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
