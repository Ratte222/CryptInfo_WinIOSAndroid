using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CommonForCryptPasswordLibrary;
namespace CryptApp
{
    public class SettingAndroid:Settings
    {
        public SettingAndroid(MyIO myIO)
        {
            this.myIO = myIO;
            _pathAppSetting = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "AppSettings.xml");
            string[] vs = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            myIO.WriteLine($"LoadSetting {LoadSetting()}");
        }
        public override E_INPUTOUTPUTMESSAGE LoadSetting()
        {
            try
            {
                #region SerializableSetting
                appSettings = new AppSettings();
                appSettings.CharStartAttributes = "#^";
                appSettings.CharStopAttributes = "#";
                appSettings.SeparateBlock = "***************************************************";
                appSettings.SearchSettingDefault = new SearchSettingDefault();
                appSettings.SearchSettingDefault.caseSensitive = false;
                appSettings.SearchSettingDefault.searchInHeader = false;
                appSettings.SearchSettingDefault.searchInTegs = false;
                appSettings.SearchSettingDefault.searchUntilFirstMatch = true;
                appSettings.SearchSettingDefault.viewServiceInformation = false;
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
    }
}
