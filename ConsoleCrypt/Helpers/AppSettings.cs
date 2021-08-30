using System;
using System.Collections.Generic;
using System.Text;
using CommonForCryptPasswordLibrary.Interfaces;
using ConsoleCrypt.WorkWithJson;
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
        public Dictionary<string, string> DirCryptFile { get; set; }
        [JsonProperty(PropertyName = "dir_decrypt_files")]
        public Dictionary<string, string> DirDecryptFile { get; set; }
        [JsonProperty(PropertyName = "keys_for_encrypted_files")]
        public Dictionary<string, string> KeysForEncryptedFiles { get; set; }
        public void Save()
        {
            //SaveData?.Invoke();
            base.Serialize(this, pathToSettings);
        }
    }
}
