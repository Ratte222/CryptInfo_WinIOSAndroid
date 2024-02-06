using CommonForCryptPasswordLibrary.Interfaces;
using CryptLibraryStandart.SymmetricCryptography;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommonForCryptPasswordLibrary.Services
{
    //ToDo: add another implementation for other operation systems
    public class CryptService_Windows : ICryptService
    {
        
        public string Encrypt(string content, string key)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(content), key));
        }

        private static byte[] Encrypt(byte[] key, string value)
        {
            SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create();
            ICryptoTransform cryptoTransform = symmetricAlgorithm.CreateEncryptor(new PasswordDeriveBytes(value, null).GetBytes(16), new byte[16]);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
            cryptoStream.Write(key, 0, key.Length);
            cryptoStream.FlushFinalBlock();
            byte[] result = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            symmetricAlgorithm.Clear();
            symmetricAlgorithm.Dispose();
            memoryStream.Dispose();
            cryptoStream.Dispose();
            cryptoTransform.Dispose();
            return result;
        }

        public string Decrypt(string content, string key)
        {
            return CryptoWithoutTry.Decrypt(content, key);
        }

        public string GetHashSHA512(string content)
        {
            var sha512 = SHA512.Create();
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);
            byte[] hash = sha512.ComputeHash(contentBytes);
            return Convert.ToBase64String(hash);
            //return CryptoWithoutTry.GetHashSHA512(content);
        }
    }
}
