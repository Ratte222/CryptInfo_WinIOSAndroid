using CommonForCryptPasswordLibrary.Interfaces;
using CryptLibraryStandart.SymmetricCryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    public class CryptService_Android : ICryptService
    {
        public string Decrypt(string content, string key)
        {
            return CryptoWithoutTry.Decrypt(content, key);
        }

        public string Encrypt(string content, string key)
        {
            return CryptoWithoutTry.Encrypt(content, key);
        }

        public string GetHashSHA512(string content)
        {
            return CryptoWithoutTry.GetHashSHA512(content);
        }
    }
}
