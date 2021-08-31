using System;
using System.Collections.Generic;
using System.Linq;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.WorkWithJson;
using Newtonsoft.Json;

namespace ConsoleCrypt.Helpers
{
    public class AppSettings : SerializeDeserializeJson<AppSettings>, IAppSettings
    {
        //public delegate void SaveSettingData();
        //public event SaveSettingData SaveData;
        [JsonIgnore]
        public readonly string pathToSettings = System.IO.Path.Combine("JSON", "AppSettings.json");
        [JsonProperty(PropertyName = "dir_crypt_files")]
        public List<FileModelInSettings> DirCryptFile { get; set; }
        [JsonProperty(PropertyName = "dir_decrypt_files")]
        public List<FileModelInSettings> DirDecryptFile { get; set; }
        [JsonProperty(PropertyName = "keys_for_encrypted_files")]
        public List<FileModelInSettings> KeysForEncryptedFiles { get; set; }
        public string default_crypr_file { get; set; }
        public FileModelInSettings DefaultCryptFile { get
            {
                return DirCryptFile.FirstOrDefault(i => i.Name.ToLower() == default_crypr_file.ToLower());
            } }
        public string default_decrypr_file { get; set; }
        public FileModelInSettings DefaultDecryptFile
        {
            get
            {
                return DirDecryptFile.FirstOrDefault(i => i.Name.ToLower() == default_decrypr_file.ToLower());
            }
        }
        public void Save()
        {
            //SaveData?.Invoke();
            base.SerializeToFile(this, pathToSettings);
        }
    }
}
