using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
namespace ConsoleCrypt.Commands
{
    [Verb("create", HelpText = "Create block or group")]
    class CreateCommand
    {
        [Option('b', "block", Required = false, HelpText = "Create block")]
        public bool Block { get; set; }

        [Option('g', "group", Required = false, HelpText = "Create group")]
        public bool Group { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
