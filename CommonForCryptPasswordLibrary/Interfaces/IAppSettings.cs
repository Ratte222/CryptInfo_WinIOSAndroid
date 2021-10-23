using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IAppSettings
    {
        public List<FileModelInSettings> DirCryptFile { get; set; }
        public List<FileModelInSettings> DirDecryptFile { get; set; }
        public List<FileModelInSettings> KeysForEncryptedFiles { get; set; }
        public FileModelInSettings SelectedCryptFile { get; }
        public string selected_crypr_file { get; set; }
        //public FileModelInSettings DefaultCryptFile { get; }
        public FileModelInSettings SelectedDecryptFile { get; }
        public string selected_decrypr_file { get; set; }
        //public FileModelInSettings DefaultDecryptFile { get; }
        void Save();
        //void Load();
    }
}
