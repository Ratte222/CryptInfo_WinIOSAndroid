using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    public interface IMainLogicService
    {
        E_INPUTOUTPUTMESSAGE LoadDefaultParams();
        bool SearchUntilFirstMach { get; }
        void Toggle_caseSensitive();
        void Toggle_searchInTegs();
        void Toggle_searchInHeader();
        void Toggle_searchUntilFirstMatch();
        void Toggle_viewServiceInformation();    
        void Toggle_searchEverywhere();
        BlockModel GetBlockData(Filter filterShow);
        List<BlockModel> GetBlockDatas(Filter filterShow);
        string InitCryptFile(string key);
        string InitCryptFiles(string key);
        //void DecryptFile();
        //void EncryptFile(string key);
    }
}
