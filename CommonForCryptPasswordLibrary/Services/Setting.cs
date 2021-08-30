using System;
using System.IO;
using System.Xml.Serialization;
using CommonForCryptPasswordLibrary.Interfaces;

namespace CommonForCryptPasswordLibrary.Services
{    
    public class Settings:ISettings
    {
        protected IAppSettings _appSettings;
        protected ISearchSettings _searchSettings;
        protected IMyIO _myIO;

        public Settings() { }

        public Settings(IAppSettings appSettings, ISearchSettings searchSettings, IMyIO myIO)
        {
            _appSettings = appSettings;
            _searchSettings = searchSettings;
            this._myIO = myIO;             
            //myIO.WriteLine($"LoadSetting {LoadSetting()}");
        }
//        public virtual E_INPUTOUTPUTMESSAGE LoadSetting()
//        {
//            try
//            {
//                #region SerializableSetting
                
//#endregion
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
//                _myIO.WriteLine(message);
//#endif
//            }
//            return E_INPUTOUTPUTMESSAGE.ExceptionLoadSetting;
//        }
        public E_INPUTOUTPUTMESSAGE SaveSetting()
        {
            try
            {
                if (_appSettings == null)
                {
                    return E_INPUTOUTPUTMESSAGE.NoLinkToSettings;
                }
                else
                {
                    _appSettings.Save();
                    _searchSettings.Save();
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
                _myIO.WriteLine(message);
#endif
            }
            return E_INPUTOUTPUTMESSAGE.ExceptionSaveSetting;
        }
        //public E_INPUTOUTPUTMESSAGE ResetSetting()
        //{
        //    E_INPUTOUTPUTMESSAGE i_ = SaveSetting();
        //    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
        //    {
        //        return i_;
        //    }
        //    return LoadSetting();
        //}        

        public E_INPUTOUTPUTMESSAGE SetDirCryptFile(string val)
        {
            //if (_appSettings == null)
            //{
            //    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
            //    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
            //        return i_;
            //}
            _appSettings.DirCryptFile.Add(Path.GetFileName(val), val);
            return E_INPUTOUTPUTMESSAGE.Ok;
        }
        public E_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val)
        {
            //if (_appSettings == null)
            //{
            //    E_INPUTOUTPUTMESSAGE i_ = LoadSetting();
            //    if (i_ != E_INPUTOUTPUTMESSAGE.Ok)
            //        return i_;
            //}
            _appSettings.DirDecryptFile.Add(Path.GetFileName(val), val);
            return E_INPUTOUTPUTMESSAGE.Ok;
        }

        public string GetDirCryptFile()
        {            
            return _appSettings.DirCryptFile["CryptTest"];
        }
        public string GetDirDecryptFile()
        {
            return _appSettings.DirDecryptFile["DecryptTest"];
        }
        //public string Get_separateBlock()
        //{
        //    return _appSettings.SeparateBlock;
        //}
        //public string Get_charStartAttributes()
        //{
        //    return _appSettings.CharStartAttributes;
        //}
        public bool Get_caseSensitive()
        {
            return _searchSettings.CaseSensitive;
        }
        public bool Get_searchInTegs()
        {
            return _searchSettings.SearchInTegs;
        }
        public bool Get_searchInHeader()
        {
            return _searchSettings.SearchInHeader;
        }
        public bool Get_searchUntilFirstMatch()
        {
            return _searchSettings.SearchUntilFirstMatch;
        }
        public bool Get_viewServiceInformation()
        {
            return _searchSettings.ViewServiceInformation;
        }
    }
}
