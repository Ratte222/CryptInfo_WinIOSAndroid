using CommonForCryptPasswordLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using AuxiliaryLib.WorkWithJson;
using CryptLibraryStandart.SymmetricCryptography;
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
        private readonly ICryptService _cryptService;
        public EncryptDecryptService(ICryptService cryptService)
        {
            _cryptService = cryptService;
        }
        public EncryptDecryptService(EncryptDecryptSettings settings, ICryptService cryptService)
        {
            _settings = settings;
            _cryptService = cryptService;
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

        private void CalculateHashSHA512ForGroupsAndBlocks(bool calculateAllHash = false)
        {
            if(calculateAllHash)
            {
                CryptFileModel.DecryptInfoContent.ForEach(i => i.HashSha512 = _cryptService.GetHashSHA512(i.ToString()));
                CryptFileModel.DecryptInfoContent.ForEach(i => i.CryptBlockModels.ForEach(
                    j=>j.HashSha512 = _cryptService.GetHashSHA512(j.ToString())));
            }
            else
            {
                var groups = CryptFileModel.DecryptInfoContent.Where(i => string.IsNullOrEmpty(i.HashSha512)).ToArray();
                var blocks = CryptFileModel.DecryptInfoContent.Select(i => i.CryptBlockModels)
                    .Select(j => j.FindAll(b => { return string.IsNullOrEmpty(b.HashSha512); }))
                    .ToArray();
                for (int i = 0; i < groups.Length; i++)
                {
                    groups[i].HashSha512 = _cryptService.GetHashSHA512(groups[i].ToString());
                }

                for (int i = 0; i < blocks.Length; i++)
                {
                    for (int j = 0; j < blocks[i].Count; j++)
                    {
                        blocks[i][j].HashSha512 = _cryptService.GetHashSHA512(blocks[i][j].ToString());
                    }
                }
            }            
        }
        /// <summary>
        /// Take data from a encrypted file and puts it in an decrypted file
        /// </summary>
        public void DecryptDataAndSaveToFile(EncryptDecryptSettings settings)
        {
            _settings = settings;
            if (string.IsNullOrEmpty(settings.DecryptPath))
                throw new ValidationException($"Path to decrypt file is null or empty");            
            if (!Directory.Exists(Path.GetDirectoryName(_settings.DecryptPath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_settings.DecryptPath));
            using(StreamWriter sw = new StreamWriter(_settings.DecryptPath))
            {
                sw.Write(DecryptData());
                sw.Flush();
            }         
        }


        /// <summary>
        /// Take data from a decrypted file and puts it in an encrypted file
        /// </summary>
        public void EncryptDataAndSaveToFile(EncryptDecryptSettings settings)
        {
            _settings = settings;
            if(string.IsNullOrEmpty(_settings.DecryptPath))
                throw new ValidationException($"Path to decrypted file is null or empty");
            string decryptData = null;
            using(StreamReader sr = new StreamReader(_settings.DecryptPath))
            {
                decryptData = sr.ReadToEnd();
            }
            try
            {
                CryptFileModel = cryptFileModel.Deserialize(decryptData);
            }
            catch (Exception ex)
            {
                if ((ex.HResult == -2146233088) && ex.Source == "Newtonsoft.Json")
                {
                    throw new TheFileIsDamagedException($"The file {_settings.DecryptPath} is damaged.\r\n" +
                        $"Check valid json. \r\n Message: {ex.Message}.");
                }
                else
                    throw ex;
            }
            EncryptAndSaveData(true);
        }


        /// <summary>
        /// Take data from <see cref="CryptFileModel"/see> calculate hash, enrcypt and
        /// puts it in the encrypted file.
        /// </summary>
        /// <param name="calculateAllHash">calculate hash for all entity</param>
        public void EncryptAndSaveData(bool calculateAllHash = false)
        {
            if (String.IsNullOrEmpty(_settings.EncryptPath))
                throw new ValidationException($"Path to encrypted file is null or empty");
            CalculateHashSHA512ForGroupsAndBlocks(calculateAllHash);
            string jsonData = listCryptGroupModel.Serialize(CryptFileModel.DecryptInfoContent);
            CryptFileModel.Hash = _cryptService.GetHashSHA512(jsonData);
            string json = cryptFileModel.Serialize(CryptFileModel);
            string encryptData = _cryptService.Encrypt(json, _settings.Key);
            using (StreamWriter sw = new StreamWriter(_settings.EncryptPath, false, Encoding.UTF8))
            {
                sw.Write(encryptData);
                sw.Flush();
            }
        }
        /// <summary>
        /// This method used in common library for crypted data
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ReadFromCryptFileException"></exception>
        /// <exception cref="TheFileIsDamagedException"></exception>
        public string DecryptData()
        {
            string encryptData = "";
            if (!File.Exists(_settings.EncryptPath))
                throw new ValidationException($"The encrypted file does not exist. Path to file: {_settings.EncryptPath}");
            using(StreamReader sr = new StreamReader(_settings.EncryptPath, Encoding.UTF8))
            {
                encryptData = sr.ReadToEnd();
            }
            string decryptData = "";
            try
            {
                decryptData = _cryptService.Decrypt(encryptData, _settings.Key);
            }
            catch(Exception ex)
            {
                if (ex.HResult == -2146233087)
                {
                    throw new ReadFromCryptFileException("Faled to decrypt data. Check password.");
                }
                else
                    throw ex;
            }
            if (!_settings.DecryptWithoutDeserialize)
            {
                try
                {
                    CryptFileModel = cryptFileModel.Deserialize(decryptData);
                }
                catch (Exception ex)
                {
                    if ((ex.HResult == -2146233088) && ex.Source == "Newtonsoft.Json")
                    {
                        throw new TheFileIsDamagedException($"The file {_settings.EncryptPath} is damaged.\r\n" +
                            $"Message: {ex.Message}.\r\n To decrypt the file, use the command: decrypt -f --withoutDeserialize");//ToDo: move this command description in Console Application
                    }
                    else
                        throw ex;
                }
                string tempJson = listCryptGroupModel.Serialize(CryptFileModel.DecryptInfoContent);
                string hash = _cryptService.GetHashSHA512(tempJson);
                if (CryptFileModel.Hash != hash)
                    FindDamagedBlocksAndGroups();
            }
            return decryptData;
        }

        private void FindDamagedBlocksAndGroups()
        {
            StringBuilder damaged = new StringBuilder();
            damaged.AppendLine($"Hash for whole file does not equal");
            foreach (var group in CryptFileModel.DecryptInfoContent)
            {
                if(group.HashSha512 != _cryptService.GetHashSHA512(group.ToString()))
                {
                    damaged.AppendLine($"Group {group.Name} (groupId: {group.Id}) is damaged");
                }
                foreach(var block in group.CryptBlockModels)
                {
                    if (block.HashSha512 != _cryptService.GetHashSHA512(block.ToString()))
                    {
                        damaged.AppendLine($"Block {block.Title} (blockId: {block.Id}, groupId: {block.GroupId}) is damaged");
                    }
                }
            }            
            throw new TheFileIsDamagedException($"The file {_settings.EncryptPath} is damaged. Damaged blocks/groups:\r\n{damaged.ToString()}");
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
