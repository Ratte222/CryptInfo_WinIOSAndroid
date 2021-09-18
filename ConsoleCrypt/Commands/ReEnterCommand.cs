using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("reenter", HelpText = "ReEnter something")]
    public class ReEnterCommand
    {
        [Option('p', "password", Required = true, HelpText = "ReEnter password", Default = false)]
        public bool ReenterPassword { get; set; }
    }
}
