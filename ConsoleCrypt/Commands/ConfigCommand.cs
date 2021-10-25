using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("config", HelpText = "Work with configuration")]
    public class ConfigCommand
    {
        [Value(1, Required = false, HelpText = "config parameter")]
        public string ConfigParameter { get; set; }

        [Value(2, Required = false, HelpText = "parameter value. Spase write as \"\\*\"")]
        public string ParameterValue { get; set; }
        [Option("openInEditor", Required = false, HelpText = "open config in selected editor", Default = false)]
        public bool OpenInEditor { get; set; }
    }
}
