using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IAppSettings
    {
        public Dictionary<string, string> DirCryptFile { get; set; }
        public Dictionary<string, string> DirDecryptFile { get; set; }
        public Dictionary<string, string> KeysForEncryptedFiles { get; set; }
        void Save();
        //void Load();
    }
}
