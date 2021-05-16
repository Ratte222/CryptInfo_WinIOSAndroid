using System;
using System.IO;
using System.Xml.Serialization;

namespace CommonForCryptPasswordLibrary
{    
    public class Settings
    {
        protected AppSettings appSettings;
        protected XmlSerializer formatter;
        protected string _pathAppSetting;
        protected MyIO myIO;
        //public bool isAndroid;
        public Settings() { }

        public Settings(MyIO myIO)
        {   
            this.myIO = myIO;
            //this.isAndroid = isAndroid;
#if ANDROID
            
                _pathAppSetting = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "AppSettings.xml");
                string[] vs = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            
#else
            
                _pathAppSetting = Path.Combine(Path.GetDirectoryName(
                    System.Reflection.Assembly.GetEntryAssembly().Location), "AppSettings.xml");
                formatter = new XmlSerializer(typeof(AppSettings));
            
#endif            
            myIO.WriteLine($"LoadSetting {LoadSetting()}");
        }
        public virtual E_INPUTOUTPUTMESSAGE LoadSetting()
        {
            try
            {
                #region SerializableSetting
#if ANDROID
                
                    
                
#else
                using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                    {
                        appSettings = (AppSettings)formatter.Deserialize(fs);
                    }
#endif
#endregion
                return E_INPUTOUTPUTMESSAGE.Ok;
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                myIO.WriteLine(message);
#endif
            }
            return E_INPUTOUTPUTMESSAGE.ExceptionLoadSetting;
        }
        public E_INPUTOUTPUTMESSAGE SaveSetting()
        {
            try
            {
                if (appSettings == null)
                {
                    return E_INPUTOUTPUTMESSAGE.NoLinkToSettings;
                }
                else
                {
                    using (FileStream fs = new FileStream(_pathAppSetting, FileMode.OpenOrCreate))
                    {
                        formatter.Serialize(fs, appSettings);
                    }
                    return E_INPUTOUTPUTMESSAGE.Ok;
                }
            }
            catch (Exception ex)
            {
                string message = $"InnerException = {ex?.InnerException?.ToString()} \r\n" +
                            $"Message = {ex?.Message?.ToString()} \r\n" +
                            $"Source = {ex?.Source?.ToString()} \r\n" +
                            $"StackTrace = {ex?.StackTrace?.ToString()} \r\n" +
                            $"TargetSite = {ex?.TargetSite?.ToString()}";
#if DEBUG
                myIO.WriteLine(message);
#endif
            }
            return E_INPUTOUTPUTMESSAGE.ExceptionSaveSetting;
        }
        public E_INPUTOUTPUTMESSAGE ResetSetting()
        {
            E_INPUTOUTPUTMESSAGE i_ = SaveSetting();
            if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
            {
                return i_;
            }
            return LoadSetting();
        }        

        public E_INPUTOUTPUTMESSAGE SetDirCryptFile(string val)
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirCryptFile = val;
            return E_INPUTOUTPUTMESSAGE.Ok;
        }
        public E_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val)
        {
            if (appSettings == null)
            {
                E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
                if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
                    return i_;
            }
            appSettings.DirDecryptFile = val;
            return E_INPUTOUTPUTMESSAGE.Ok;
        }

        public string GetDirCryptFile()
        {            
            return appSettings.DirCryptFile;
        }
        public string GetDirDecryptFile()
        {
            return appSettings.DirDecryptFile;
        }
        public string Get_separateBlock()
        {
            return appSettings.SeparateBlock;
        }
        public string Get_charStartAttributes()
        {
            return appSettings.CharStartAttributes;
        }
        public bool Get_caseSensitive()
        {
            return appSettings.SearchSettingDefault.caseSensitive;
        }
        public bool Get_searchInTegs()
        {
            return appSettings.SearchSettingDefault.searchInTegs;
        }
        public bool Get_searchInHeader()
        {
            return appSettings.SearchSettingDefault.searchInHeader;
        }
        public bool Get_searchUntilFirstMatch()
        {
            return appSettings.SearchSettingDefault.searchUntilFirstMatch;
        }
        public bool Get_viewServiceInformation()
        {
            return appSettings.SearchSettingDefault.viewServiceInformation;
        }
    }
}
