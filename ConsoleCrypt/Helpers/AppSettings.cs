using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.WorkWithJson;
using Newtonsoft.Json;

namespace ConsoleCrypt.Helpers
{
    public class AppSettings : SerializeDeserializeJson<AppSettings>, IAppSettings
    {
        //Assembly.GetExecutingAssembly().Location in net5 and letter returned null!!!!!!
        [JsonIgnore]
        public readonly string pathToSettings = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JSON", "AppSettings.json");
        [JsonProperty(PropertyName = "dir_crypt_files")]
        public List<FileModelInSettings> DirCryptFile { get; set; }
        [JsonProperty(PropertyName = "dir_decrypt_files")]
        public List<FileModelInSettings> DirDecryptFile { get; set; }
        [JsonProperty(PropertyName = "keys_for_encrypted_files")]
        public List<FileModelInSettings> KeysForEncryptedFiles { get; set; }
        public string selected_crypr_file { get; set; }
        public FileModelInSettings SelectedCryptFile { get
            {
                return DirCryptFile.FirstOrDefault(i => i.Name.ToLower() == selected_crypr_file.ToLower());
            } }
        public string selected_decrypr_file { get; set; }
        public FileModelInSettings SelectedDecryptFile
        {
            get
            {
                return DirDecryptFile.FirstOrDefault(i => i.Name.ToLower() == selected_decrypr_file.ToLower());
            }
        }

        public void Save()
        {
            //SaveData?.Invoke();
            base.SerializeToFile(this, pathToSettings);
        }
    }
}
