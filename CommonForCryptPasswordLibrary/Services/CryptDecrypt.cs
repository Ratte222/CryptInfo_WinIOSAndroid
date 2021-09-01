using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary.WorkWithJson;
using CryptLibrary;
using Newtonsoft.Json;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Exceptions;
using System.IO;

namespace CommonForCryptPasswordLibrary.Services
{
    public class CryptDecrypt:ICryptDecrypt
    {
        public CryptFileModel CryptFileModel { get; set; }
        private SerializeDeserializeJson<List<CryptGroupModel>> listCryptGroupModel = new SerializeDeserializeJson<List<CryptGroupModel>>();
        private SerializeDeserializeJson<CryptFileModel> cryptFileModel = new SerializeDeserializeJson<CryptFileModel>();
        private CryptDecryptSettings _settings;
        public CryptDecrypt() { }
        public CryptDecrypt(CryptDecryptSettings settings)
        {
            _settings = settings;
        }

        public void LoadData(CryptDecryptSettings settings)
        {
            _settings = settings;
            DecryptData();
        }

        public void SaveChanges()
        {
            EncryptAndSaveData();
        }

        public void EncryptAndSaveData()
        {
            if (String.IsNullOrEmpty(_settings.Path))
                throw new ValidationException("path is null");
            string jsonData = listCryptGroupModel.Serialize(CryptFileModel.DecryptInfoContent);
            CryptFileModel.Hash = CryptoWithoutTry.GetHashSHA512(jsonData);
            string json = cryptFileModel.Serialize(CryptFileModel);
            string encryptData = CryptoWithoutTry.Encrypt(json, _settings.Key);
            using (StreamWriter sw = new StreamWriter(_settings.Path, false, Encoding.UTF8))
            {
                sw.Write(encryptData);
                sw.Flush();
            }
        }

        public void DecryptData()
        {
            string encryptData = "";
            using(StreamReader sr = new StreamReader(_settings.Path, Encoding.UTF8))
            {
                encryptData = sr.ReadToEnd();
            }
            string decryptData = CryptoWithoutTry.Decrypt(encryptData, _settings.Key);
            CryptFileModel = cryptFileModel.Deserialize(decryptData);
            string tempJson = listCryptGroupModel.Serialize(CryptFileModel.DecryptInfoContent);
            string hash = CryptoWithoutTry.GetHashSHA512(tempJson);
            if (CryptFileModel.Hash != hash)
                throw new TheFileIsDamagedException($"The file {_settings.Path} is damaged");
        }

        public void GetInitData()
        {
            CryptFileModel = new CryptFileModel();
            CryptGroupModel group1 = new CryptGroupModel()
            {
                Id = Guid.NewGuid(),
                Name = "SocialWide",
                Description = "description"
            };
            group1.CryptBlockModels.AddRange(new[]
            {
                new CryptBlockModel(){Id = Guid.NewGuid(), GroupId = group1.Id, Title = "Picabu", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" },
                new CryptBlockModel(){Id = Guid.NewGuid(), GroupId = group1.Id, Title = "Instagram", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" }
            });
            CryptGroupModel group2 = new CryptGroupModel()
            {
                Id = Guid.NewGuid(),
                Name = "Work",
                Description = "description"
            };
            group2.CryptBlockModels.AddRange(new[]
            {
                new CryptBlockModel(){Id = Guid.NewGuid(), GroupId = group2.Id, Title = "Google", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" },
                new CryptBlockModel(){Id = Guid.NewGuid(), GroupId = group2.Id, Title = "LinkedIn", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" }
            });
            CryptFileModel.DecryptInfoContent.Add(group1);
            CryptFileModel.DecryptInfoContent.Add(group2);
            
        }
    }
}
