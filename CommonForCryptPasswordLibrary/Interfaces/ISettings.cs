using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ISettings
    {
        //E_INPUTOUTPUTMESSAGE LoadSetting();
        E_INPUTOUTPUTMESSAGE SaveSetting();
        //E_INPUTOUTPUTMESSAGE ResetSetting();
        E_INPUTOUTPUTMESSAGE SetDirCryptFile(string val);
        E_INPUTOUTPUTMESSAGE SetDirDecryptFile(string val);
        public string GetDirCryptFile();
        string GetDirDecryptFile();
        //string Get_separateBlock();
        //string Get_charStartAttributes();
        bool Get_caseSensitive();
        bool Get_searchInTegs();
        bool Get_searchInHeader();
        bool Get_searchUntilFirstMatch();
        bool Get_viewServiceInformation();

    }
}
