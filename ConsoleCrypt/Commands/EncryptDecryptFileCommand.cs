using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.Commands
{
    [Verb("encdec_asm", HelpText = "encrypt decrypt file using RSA")]
    public class EncryptDecryptFileCommand
    {
        [Option("createKey", Required = false, HelpText = "create asymmetric asymetrical key", Default = false)]
        public bool CreateKey { get; set; }

        [Option("decrypt", Required = false, HelpText = "decrypts the file using asymetrical key", Default = false)]
        public bool Decrypt { get; set; }

        [Option("encrypt", Required = false, HelpText = "decrypts the file using asymetrical key", Default = false)]
        public bool Encrypt { get; set; }

        [Option("exportPublicKey", Required = false, HelpText = "export public key to file")]
        public bool ExportPublicKey { get; set; }

        [Option("importPublicKey", Required = false, HelpText = "import public key from file")]
        public bool ImporPublicKey { get; set; }

        [Option("key", Required = false, HelpText = "asymmetric key")]
        public string AsymmetricKey { get; set; }

        [Option("pathFrom", Required = false, HelpText = "Path to file")]
        public string PathFrom { get; set; }

        [Option("pathTo", Required = false, HelpText = "Path to file")]
        public string PathTo { get; set; }
    }
}
