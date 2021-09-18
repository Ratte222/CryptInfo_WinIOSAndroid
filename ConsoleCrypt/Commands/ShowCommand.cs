using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace ConsoleCrypt.Commands
{
    [Verb("show", HelpText = "Show something")]
    public class ShowCommand
    {
        [Option("cs", Required = false, HelpText = "Case sensetive", Default = false)]
        public bool CaseSensetive { get; set; }

        [Option("allgroups", Required = false, HelpText = "Show all groups in crypt file. Do not consider parameters: [-g], [--allblocks], [--cs]", Default = true)]
        public bool AllGroups { get; set; }

        [Option("allblocks", Required = false, HelpText = "Show all blocks and groups in crypt file. Do not consider parameters: [-g], [--allgroups], [--cs]", Default = false)]
        public bool AllBlocks { get; set; }

        //[Value(0)]
        //public string Command { get; set; }
        [Option('b', "block", Required = false, HelpText = "Show blocks. Can be used with the parameters: [-g], [-p], [--cs]")]
        public string Block { get; set; }

        [Option('g', "group", Required = false, HelpText = "Show block in group")]
        public string Group { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }
}
