using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IEncryptDecryptService
    {
        CryptFileModel CryptFileModel { get; set; }
        void LoadData(EncryptDecryptSettings settings);
        void SaveChanges();
        void DecryptDataAndSaveToFile(EncryptDecryptSettings settings);
        void EncryptDataAndSaveToFile(EncryptDecryptSettings settings);
    }
}
