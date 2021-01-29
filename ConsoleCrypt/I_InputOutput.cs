using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt
{
    enum I_INPUTOUTPUTMESSAGE
    {
        Ok = 0,
        False,
        NoLinkToSettings,
        DecryptFileNotExist,
        CryptFileNotExist,
        KeyIsNull,
        ExceprionCryptFile,
        ExceptionLoadSetting,
        ExceptionSaveSetting,
        ExceprionDecryptFile,
        SearchBlockFromCryptRepositoriesUseKeyWord
    };
    interface I_InputOutput
    {
        //protected AppSettings appSettings { get; set; }
        //void Init(AppSettings _appSettings)
        //{
        //    appSettings = _appSettings;
        //}
        I_INPUTOUTPUTMESSAGE LoadSetting();
        I_INPUTOUTPUTMESSAGE SaveSetting();
        I_INPUTOUTPUTMESSAGE ResetSetting();
        I_INPUTOUTPUTMESSAGE SetDirCryptFile(string val);
        I_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val);
        string GetDirDecryptFile();
        string GetDirCryptFile();
        I_INPUTOUTPUTMESSAGE CryptFile(string key);
        I_INPUTOUTPUTMESSAGE DecryptFile(string key);
        I_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord, bool caseSensitive);
        void ShowAPersone(string message);
        string ReadFromPersone();
    }
}
