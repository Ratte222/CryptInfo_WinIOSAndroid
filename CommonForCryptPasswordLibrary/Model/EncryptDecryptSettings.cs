using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class EncryptDecryptSettings
    {
        public string Key { get; set; }
        public string EncryptPath { get; set; }
        public string DecryptPath { get; set; }
        public bool DecryptWithoutDeserialize { get; set; }
    }
}
