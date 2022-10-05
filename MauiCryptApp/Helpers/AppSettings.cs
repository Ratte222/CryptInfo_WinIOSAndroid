using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Helpers
{
    public class AppSettings:IAppSettings
    {
        public List<FileModelInSettings> DirCryptFile { get; set; }
        public List<FileModelInSettings> DirDecryptFile { get; set; }
        public List<FileModelInSettings> KeysForEncryptedFiles { get; set; }
        public string selected_crypr_file { get; set; }
        public FileModelInSettings SelectedCryptFile
        {
            get
            {
                return DirCryptFile.FirstOrDefault(i => i.Name.ToLower() == selected_crypr_file.ToLower());
            }
        }
        public string selected_decrypr_file { get; set; }
        
        public FileModelInSettings SelectedDecryptFile
        {
            get
            {
                return DirDecryptFile.FirstOrDefault(i => i.Name.ToLower() == selected_decrypr_file.ToLower());
            }
        }
        public string Editor { get; set; }
        public void Save()
        {
            //SaveData?.Invoke();
            //base.SerializeToFile(this, PathToSettings);
        }
    }
}
