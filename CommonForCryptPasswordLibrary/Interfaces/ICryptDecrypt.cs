using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ICryptDecrypt
    {
        CryptFileModel CryptFileModel { get; set; }
        void LoadData(CryptDecryptSettings settings);
        void SaveChanges();
    }
}
