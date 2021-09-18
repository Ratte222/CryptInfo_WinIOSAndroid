using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
namespace ConsoleCrypt.Commands
{
    [Verb("update", HelpText = "Update block or group")]
    public class UpdateCommand
    {
        [Option('b', "block", Required = false, HelpText = "Update block")]
        public string Block { get; set; }

        [Option('g', "group", Required = true, HelpText = "Update group")]
        public string Group { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
