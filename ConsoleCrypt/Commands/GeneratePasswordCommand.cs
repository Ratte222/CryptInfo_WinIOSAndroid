using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("generatepassword", HelpText = "Use for generate password")]
    public class GeneratePasswordCommand
    {
        [Option('l', "length", Required = true, HelpText = "Length of generated password")]
        public int Length { get; set; }
    }
}
