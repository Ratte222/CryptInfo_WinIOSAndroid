using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary.WorkWithJson;
using CryptLibrary;
using Newtonsoft.Json;
using System.Linq;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Exceptions;
using System.IO;

namespace CommonForCryptPasswordLibrary.Services
{
    public class EncryptDecryptService:IEncryptDecryptService
    {
        public CryptFileModel CryptFileModel { get; set; }
        private SerializeDeserializeJson<List<GroupModel>> listCryptGroupModel = new SerializeDeserializeJson<List<GroupModel>>();
        private SerializeDeserializeJson<CryptFileModel> cryptFileModel = new SerializeDeserializeJson<CryptFileModel>();
        private EncryptDecryptSettings _settings;
        public EncryptDecryptService() { }
        public EncryptDecryptService(EncryptDecryptSettings settings)
        {
            _settings = settings;
        }

        public void LoadData(EncryptDecryptSettings settings)
        {
            _settings = settings;
            DecryptData();
        }

        public void SaveChanges()
        {
            EncryptAndSaveData();
        }

        private void CalculateHashSHA512ForGroupsAndBlocks()
        {
            var groups = CryptFileModel.DecryptInfoContent.Where(i => string.IsNullOrEmpty(i.HashSha512)).ToArray();
            var blocks = CryptFileModel.DecryptInfoContent.Where(j => j.CryptBlockModels.Any(b => string.IsNullOrEmpty(b.HashSha512)))
                .Select(i=>i.CryptBlockModels).ToArray();
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].HashSha512 = CryptoWithoutTry.GetHashSHA512(groups[i].ToString());
            }

            for (int i = 0; i < blocks.Length; i++)
            {
                for (int j = 0; j < blocks[i].Count; j++)
                {
                    blocks[i][j].HashSha512 = CryptoWithoutTry.GetHashSHA512(blocks[i][j].ToString());
                }
            }
        }

        public void EncryptAndSaveData()
        {
            if (String.IsNullOrEmpty(_settings.Path))
                throw new ValidationException("path is null");
            CalculateHashSHA512ForGroupsAndBlocks();
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
            if (!File.Exists(_settings.Path))
                throw new ValidationException($"The encrypted file does not exist. Path to file: {_settings.Path}");
            using(StreamReader sr = new StreamReader(_settings.Path, Encoding.UTF8))
            {
                encryptData = sr.ReadToEnd();
            }
            string decryptData = "";
            try
            {
                decryptData = CryptoWithoutTry.Decrypt(encryptData, _settings.Key);
            }
            catch(Exception ex)
            {
                if(ex.HResult == -2146233087)
                {
                    throw new ReadFromCryptFileException("Faled to decrypt data. Check password.");
                }
            }
            CryptFileModel = cryptFileModel.Deserialize(decryptData);
            string tempJson = listCryptGroupModel.Serialize(CryptFileModel.DecryptInfoContent);
            string hash = CryptoWithoutTry.GetHashSHA512(tempJson);
            if (CryptFileModel.Hash != hash)
                throw new TheFileIsDamagedException($"The file {_settings.Path} is damaged");
        }

        public void GetInitData()
        {
            CryptFileModel = new CryptFileModel();
            GroupModel group1 = new GroupModel()
            {
                Id = Guid.NewGuid(),
                Name = "SocialWide",
                Description = "description"
            };
            group1.CryptBlockModels.AddRange(new[]
            {
                new BlockModel(){Id = Guid.NewGuid(), GroupId = group1.Id, Title = "Picabu", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" },
                new BlockModel(){Id = Guid.NewGuid(), GroupId = group1.Id, Title = "Instagram", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" }
            });
            GroupModel group2 = new GroupModel()
            {
                Id = Guid.NewGuid(),
                Name = "Work",
                Description = "description"
            };
            group2.CryptBlockModels.AddRange(new[]
            {
                new BlockModel(){Id = Guid.NewGuid(), GroupId = group2.Id, Title = "Google", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" },
                new BlockModel(){Id = Guid.NewGuid(), GroupId = group2.Id, Title = "LinkedIn", Email="yourEmail@gmail.com", Password="12345678", UserName="YourUserName" }
            });
            CryptFileModel.DecryptInfoContent.Add(group1);
            CryptFileModel.DecryptInfoContent.Add(group2);
            
        }
    }
}
