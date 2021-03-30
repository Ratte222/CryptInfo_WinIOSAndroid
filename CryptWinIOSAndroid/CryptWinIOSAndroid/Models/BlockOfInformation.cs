using System;
using System.Collections.Generic;
using System.Text;

namespace CryptWinIOSAndroid.Models
{
    public class BlockOfInformation
    {
        _Problem problem = new _Problem();
        string service;
        List<string> contents;
        public string EncryptBlock;
        int indexBlock;
        int BlockStartIndex;
        string CryptoKey;
        string CryptoKeyImp;
        string MainKey;
        string MainImportantKey;
        bool BlockModifi = false;
        bool CSDoesNotExist = false;
        //public Item Item = new Item();
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public BlockOfInformation(int _indexBlock, int _BlockStartIndex,
        string _CryptoKey, string _CryptoKeyImp)
        {
            indexBlock = _indexBlock;
            BlockStartIndex = _BlockStartIndex;
            CryptoKey = _CryptoKey;
            CryptoKeyImp = _CryptoKeyImp;
            //Decrypt()
        }
        public BlockOfInformation(string[] _content, int _indexBlock,
            string _CryptoKey, string _CryptoKeyImp, string _MainKey,
            string _MainInmortantKey)
        {
            indexBlock = _indexBlock;
            CryptoKey = _CryptoKey;
            CryptoKeyImp = _CryptoKeyImp;
            MainKey = _MainKey;
            MainImportantKey = _MainInmortantKey;
            EncryptBlock = Encrypt(_content);
        }



        public string Encrypt(string[] content)//уже есть служебная часть
        {
            string temp = "";
            for (int i = 1; i < content.Length; i++)
            {
                temp += content[i];
            }
            content[0] = $"service {content.Length} {CryptoWithoutTry.CalculateCSString(temp)}";//services information
            temp = "";
            string key = GenerateAKey();
            for (int i = 0; i < content.Length; i++)
            {
                temp += $"{CryptoWithoutTry.Encrypt(content[i], key, Encoding.UTF8)}\r\n";
                if(i == 0) { Description = temp; }
            }
            Text = temp.Substring(Description.Length);
            return temp;
        }

        public string Decrypt()
        {
            string key = GenerateAKeyBlock();
            string[] serviceBlockSplit = GetServiceInformationBlock();
            string contentsBlock = "";
            int index = Convert.ToInt32(serviceBlockSplit[1]) + BlockStartIndex;
            for (int i = BlockStartIndex + 1; i < index; i++)
            {
                contentsBlock += CryptoWithoutTry.Decrypt(Service.fileContentCryptSplit[i], key) + "\r\n";
            }
            if (serviceBlockSplit[2] != CryptoWithoutTry.CalculateCSString(contentsBlock.Replace("\r\n", "")))
            {
                problem.HasProblem = true;
                problem.ProblrmMessage = "CS does not exist";
            }
            return contentsBlock;
        }

        private string GenerateAKeyBlock()
        {
            string key = MainKey + CryptoKey;
            if (!String.IsNullOrEmpty(CryptoKeyImp))
            {
                key += MainImportantKey + CryptoKeyImp;
            }
            return key;
        }

        private string[] GetServiceInformationBlock()
        {
            return CryptoWithoutTry.Decrypt(Service.fileContentCryptSplit[BlockStartIndex],
                GenerateAKeyBlock()).Split(' ');
        }

        private string GenerateAKey()
        {
            string key = MainKey + CryptoKey;
            if (!String.IsNullOrEmpty(CryptoKeyImp))
            {
                key += MainImportantKey + CryptoKeyImp;
            }
            return key;
        }


        private string CreateKey(bool important = true)
        {
            Random random = new Random();
            string result = "";
            for (int i = 0; i < 6; i++)
            {
                result += Encoding.UTF8.GetString(new byte[] { Convert.ToByte(random.Next(48, 122)) });
            }
            return result;
        }
    }
}
