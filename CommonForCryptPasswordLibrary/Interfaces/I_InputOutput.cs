using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public enum E_INPUTOUTPUTMESSAGE
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
        SearchBlockFromCryptRepositoriesUseKeyWord,
        WriteToEndCryptFile,
        Update,
        Insert
    };
    public interface I_InputOutput
    {
        //protected AppSettings appSettings { get; set; }
        //void Init(AppSettings _appSettings)
        //{
        //    appSettings = _appSettings;
        //}
        
        //E_INPUTOUTPUTMESSAGE LoadSetting();
        E_INPUTOUTPUTMESSAGE LoadDefaultParams();
        //E_INPUTOUTPUTMESSAGE SaveSetting();
        //E_INPUTOUTPUTMESSAGE ResetSetting();
        //E_INPUTOUTPUTMESSAGE SetDirCryptFile(string val);
        //E_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val);
        //string GetDirDecryptFile();
        //string GetDirCryptFile();
        //bool Get_caseSensitive();
        //bool Get_searchInTegs();
        //bool Get_searchInHeader();
        //bool Get_searchUntilFirstMatch();
        //bool Get_viewServiceInformation();
        void Toggle_caseSensitive();
        void Toggle_searchInTegs();
        void Toggle_searchInHeader();
        void Toggle_searchUntilFirstMatch();
        void Toggle_viewServiceInformation();        
        E_INPUTOUTPUTMESSAGE CryptFile(string key);
        E_INPUTOUTPUTMESSAGE DecryptFile(string key);
        E_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord);
        E_INPUTOUTPUTMESSAGE ShowAllFromCryptFile(string key);
        E_INPUTOUTPUTMESSAGE WriteToEndCryptFile(string key, string data);
        E_INPUTOUTPUTMESSAGE Update(string key, string data, int[] blockData);
        string GetBlockData(out int[] blockData, string key, int block, int targetLine = -1);
        //void ShowAPersone(string message);
        //string ReadFromPersone();

    }
}
