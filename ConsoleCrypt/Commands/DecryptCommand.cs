using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("decrypt", HelpText = "Decrypt something")]
    public class DecryptCommand
    {

        [Option('f', "file", Required = false, HelpText = "Decrypt file", Default = false)]
        public bool File { get; set; }
        [Option("withoutDeserialize", Required = false, HelpText = "Decrypt file without json deserialize", Default = false)]
        public bool WithoutDeserialize { get; set; }

        [Option("pathFrom", Required = false, HelpText = "Path to encrypted file")]
        public string PathFrom { get; set; }

        [Option("pathFrom", Required = false, HelpText = "Path to encrypted file", Group = "")]
        public string PathTo { get; set; }

        //[Option("DecryptedFileFromSetting", Required = false, HelpText = "Decrypt selected in setting file", Default = false)]
        //public bool DecryptedFileFromSetting { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
