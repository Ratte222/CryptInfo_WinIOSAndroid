using CommonForCryptPasswordLibrary.Interfaces;
using CryptLibraryStandart.SymmetricCryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonForCryptPasswordLibrary.Services
{
    //ToDo: add another implementation for other operation systems
    public class CryptService_Windows : ICryptService
    {
        
        public string Encrypt(string content, string key)
        {
            return CryptoWithoutTry.Encrypt(content, key);
        }

        public string Decrypt(string content, string key)
        {
            return CryptoWithoutTry.Decrypt(content, key);
        }

        public string GetHashSHA512(string content)
        {
            return CryptoWithoutTry.GetHashSHA512(content);
        }
    }
}
