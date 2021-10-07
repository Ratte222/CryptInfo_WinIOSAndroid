using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonForCryptPasswordLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using Newtonsoft.Json;

namespace CryptApp
{
    public class SettingAndroid: IAppSettings
    {

        [JsonIgnore]
        public readonly string pathToSettings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "AppSettings.xml");
        [JsonProperty(PropertyName = "dir_crypt_files")]
        public List<FileModelInSettings> DirCryptFile { get; set; }
            = new List<FileModelInSettings>();
        [JsonProperty(PropertyName = "dir_decrypt_files")]
        public List<FileModelInSettings> DirDecryptFile { get; set; }
            = new List<FileModelInSettings>();
        [JsonProperty(PropertyName = "keys_for_encrypted_files")]
        public List<FileModelInSettings> KeysForEncryptedFiles { get; set; }
            = new List<FileModelInSettings>();
        public string default_crypr_file { get; set; }
        public FileModelInSettings SelectedCryptFile
        {
            get
            {
                return DirCryptFile.FirstOrDefault(i => i.Name.ToLower() == default_crypr_file.ToLower());
            }
        }
        public string default_decrypr_file { get; set; }
        public FileModelInSettings SelectedDecryptFile
        {
            get
            {
                return DirDecryptFile.FirstOrDefault(i => i.Name.ToLower() == default_decrypr_file.ToLower());
            }
        }

        public void Save()
        {
            //SaveData?.Invoke();
            //base.SerializeToFile(this, pathToSettings);
        }

        //public SettingAndroid(MyIO myIO)
        //{
        //    this.myIO = myIO;
        //    _pathAppSetting = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        //            "AppSettings.xml");
        //    string[] vs = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        //    myIO.WriteLine($"LoadSetting {LoadSetting()}");
        //}

        //        public override E_INPUTOUTPUTMESSAGE LoadSetting()
        //        {
        //            try
        //            {
        //                #region SerializableSetting
        //                appSettings = new AppSettings();
        //                appSettings.CharStartAttributes = "#^";
        //                appSettings.CharStopAttributes = "#";
        //                appSettings.SeparateBlock = "***************************************************";
        //                appSettings.SearchSettingDefault = new SearchSettingDefault();
        //                appSettings.SearchSettingDefault.caseSensitive = false;
        //                appSettings.SearchSettingDefault.searchInHeader = false;
        //                appSettings.SearchSettingDefault.searchInTegs = false;
        //                appSettings.SearchSettingDefault.searchUntilFirstMatch = true;
        //                appSettings.SearchSettingDefault.viewServiceInformation = false;
        //                #endregion
        //                return E_INPUTOUTPUTMESSAGE.Ok;
        //            }
        //            catch (Exception ex)
        //            {
        //                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
        //                            $"Message = {ex?.Message?.ToString()} \r\n" +
        //                            $"Source = {ex?.Source?.ToString()} \r\n" +
        //                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
        //                            $"TargetSite = {ex?.TargetSite?.ToString()}";
        //#if DEBUG
        //                myIO.WriteLine(message);
        //#endif
        //            }
        //            return E_INPUTOUTPUTMESSAGE.ExceptionLoadSetting;
        //        }
    }
}
