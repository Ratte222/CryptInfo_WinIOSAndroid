using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("switch", HelpText = "")]
    class SwitchCommand
    {
        [Option("epf", Required = false, HelpText = "encryp password file", Default = false)]
        public bool EncryptPasswordFile { get; set; }
        [Option("dpf", Required = false, HelpText = "decryp password file", Default = false)]
        public bool DecryptPasswordFile { get; set; }
        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
        [Value(1, Required = true, HelpText = "name file in settings")]
        public string Name { get; set; }
    }
}
