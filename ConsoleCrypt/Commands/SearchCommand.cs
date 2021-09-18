using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace ConsoleCrypt.Commands
{
    //https://codingblog.carterdan.net/2019/04/12/command-line-parser/

    [Verb("search", HelpText = "Search in crypt file")]
    public class SearchCommand
    {
        [Option("cs", Required = false, HelpText = "Case sensetive", Default = false)]
        public bool CaseSensetive { get; set; }

        [Option("sufm", Required = false, HelpText = "Search until first match", Default = true)]
        public bool SearchUntilFirstMatch { get; set; }

        [Option("se", Required = false, HelpText = "Search everywhere", Default = false)]
        public bool SearchEverywhere{ get; set; }

        //[Value(0, Hidden =true)]
        //public string Command { get; set; }

        [Value(1, Required = true, HelpText = "Search keywords")]
        public string KeyWord { get; set; }

        [Option('p', "password", Required = false, HelpText = "Entered password")]
        public string Password { get; set; }
    }

   
}
