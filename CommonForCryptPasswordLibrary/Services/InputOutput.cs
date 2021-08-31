using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using WorkWithFileLibrary;
using CryptLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;

namespace CommonForCryptPasswordLibrary.Services
{
    
    public class InputOutputFile:I_InputOutput
    {
        IMyIO console_IO;
        Encoding _encoding = Encoding.UTF8;
        IAppSettings _appSettings;
        ISearchSettings _searchSettings;
        ICryptGroup _cryptGroup;
        ICryptBlock _cryptBlock;
        public E_INPUTOUTPUTMESSAGE lastProblem { get; protected set; } = E_INPUTOUTPUTMESSAGE.Ok;
        
        
        private bool caseSensitive = false,
            searchInTegs = false,
            searchInHeader = false,
            searchUntilFirstMatch = false,
            viewServiceInformation = false,
            showAllFromCryptFile = false;

        public InputOutputFile(IMyIO _console_IO, IAppSettings appSettings,
            ISearchSettings searchSettings/*, ICryptGroup cryptGroup, ICryptoBlock cryptoBlock*/)
        {
            console_IO = _console_IO;
            _appSettings = appSettings;
            _searchSettings = searchSettings;
            //_cryptGroup = cryptGroup;
            //_cryptoBlock = cryptoBlock;
            LoadDefaultParams();
        }

        public E_INPUTOUTPUTMESSAGE LoadDefaultParams()
        {
            caseSensitive = _searchSettings.CaseSensitive;
            searchInTegs = _searchSettings.SearchInTegs;
            searchInHeader = _searchSettings.SearchInHeader;
            searchUntilFirstMatch = _searchSettings.SearchUntilFirstMatch;
            viewServiceInformation = _searchSettings.ViewServiceInformation;
            return E_INPUTOUTPUTMESSAGE.Ok;
        }

        
        public void Toggle_caseSensitive()
        {
            caseSensitive = !caseSensitive;
        }
        public void Toggle_searchInTegs()
        {
            searchInTegs = !searchInTegs;
        }
        public void Toggle_searchInHeader()
        {
            searchInHeader = !searchInHeader;
        }
        public void Toggle_searchUntilFirstMatch()
        {
            searchUntilFirstMatch = !searchUntilFirstMatch;
        }
        public void Toggle_viewServiceInformation()
        {
            viewServiceInformation = !viewServiceInformation;
        }

        public E_INPUTOUTPUTMESSAGE CryptFile(string key)
        {
            throw new NotImplementedException();
        }

        public E_INPUTOUTPUTMESSAGE DecryptFile(string key)
        {
            throw new NotImplementedException();
        }

        public E_INPUTOUTPUTMESSAGE SearchBlockFromCryptRepositoriesUseKeyWord(string key, string keyWord)
        {
            throw new NotImplementedException();
        }

        public E_INPUTOUTPUTMESSAGE ShowAllFromCryptFile(string key)
        {
            throw new NotImplementedException();
        }

        public E_INPUTOUTPUTMESSAGE WriteToEndCryptFile(string key, string data)
        {
            throw new NotImplementedException();
        }

        public E_INPUTOUTPUTMESSAGE Update(string key, string data, int[] blockData)
        {
            throw new NotImplementedException();
        }

        public string GetBlockData(out int[] blockData, string key, int block, int targetLine = -1)
        {
            throw new NotImplementedException();
        }

        public void InitCryptFiles(string key)
        {
            foreach(var path in _appSettings.DirCryptFile)
            {
                if (!System.IO.File.Exists(path.Path))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(path.Path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(path.Path));
                    CryptDecrypt cryptDecrypt = new CryptDecrypt(
                        new CryptDecryptSettings() { Key = key, Path = path.Path });
                    cryptDecrypt.GetInitData();
                    cryptDecrypt.EncryptAndSaveData();
                }
            }
            
        }

        public void InitDecryptFiles(string key)
        {
            throw new NotImplementedException();
        }

        
    }
}
