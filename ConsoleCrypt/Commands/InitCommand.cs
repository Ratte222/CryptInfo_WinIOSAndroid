using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("init", HelpText = "Inti something")]
    public class InitCommand
    {
        [Option("encryptedfile", Required = false, HelpText = "init selected encrypted file", Default = false)]
        public bool EncryptedFile { get; set; }

        [Option("encryptedfiles", Required = false, HelpText = "init selected encrypted files", Default = false)]
        public bool EncryptedFiles { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
