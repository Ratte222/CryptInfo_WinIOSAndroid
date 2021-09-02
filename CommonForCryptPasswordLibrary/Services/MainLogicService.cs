using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using WorkWithFileLibrary;
using CryptLibrary;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Filters;
using System.Runtime.InteropServices;

namespace CommonForCryptPasswordLibrary.Services
{
    [StructLayout(LayoutKind.Auto)]//default StructLayout = LayoutKind.Auto. This added for example
    public class MainLogicService:IMainLogicService
    {
        IMyIO console_IO;
        Encoding _encoding = Encoding.UTF8;
        IAppSettings _appSettings;
        ISearchSettings _searchSettings;
        IGroupService _cryptGroup;
        IBlockService _cryptBlock;
        public E_INPUTOUTPUTMESSAGE lastProblem { get; protected set; } = E_INPUTOUTPUTMESSAGE.Ok;
        
        
        private bool caseSensitive = false,
            searchInTegs = false,
            searchInHeader = false,
            searchUntilFirstMatch = false,
            viewServiceInformation = false,
            showAllFromCryptFile = false,
            searchEverywhere = false;

        public MainLogicService(IMyIO _console_IO, IAppSettings appSettings,
            ISearchSettings searchSettings, IGroupService cryptGroup, IBlockService cryptoBlock)
        {
            console_IO = _console_IO;
            _appSettings = appSettings;
            _searchSettings = searchSettings;
            _cryptGroup = cryptGroup;
            _cryptBlock = cryptoBlock;
            LoadDefaultParams();
        }

        public E_INPUTOUTPUTMESSAGE LoadDefaultParams()
        {
            caseSensitive = _searchSettings.CaseSensitive;
            searchInTegs = _searchSettings.SearchInTegs;
            searchInHeader = _searchSettings.SearchInHeader;
            searchUntilFirstMatch = _searchSettings.SearchUntilFirstMatch;
            viewServiceInformation = _searchSettings.ViewServiceInformation;
            searchEverywhere = _searchSettings.SearchEverywhere;
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

        public void Toggle_searchEverywhere()
        {
            searchEverywhere = !searchEverywhere;
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

        public BlockModel GetBlockData(Filter filterShow)
        {
            IQueryable<BlockModel> query = null;
            if(!String.IsNullOrEmpty(filterShow.GroupName))
            {
                if (caseSensitive)
                {
                    query = _cryptGroup.Get(i => i.Name.ToLower().Contains(filterShow.GroupName.ToLower()))
                      .CryptBlockModels.AsQueryable();
                }
                else
                {
                    _cryptGroup.Get(i => i.Name.ToLower().Contains(filterShow.GroupName.ToLower()))
                        .CryptBlockModels.AsQueryable();
                }                
            }
            else
            {
                query = _cryptBlock.GetAll_Queryable();
            }
            if (!caseSensitive&&!searchEverywhere)
            {
                return query.FirstOrDefault(i => i.Title.ToLower().Contains(filterShow.BlockName.ToLower()));
            }
            else if (!caseSensitive && searchEverywhere)
            {
                return query.FirstOrDefault(i => i.ToString().ToLower().Contains(filterShow.BlockName.ToLower()));
            }
            else if (caseSensitive && !searchEverywhere)
            {
                return query.FirstOrDefault(i => i.Title.Contains(filterShow.BlockName));
            }
            else// if (!caseSensitive && searchEverywhere)
            {
                return query.FirstOrDefault(i => i.ToString().Contains(filterShow.BlockName));
            }
        }

        public List<BlockModel> GetBlockDatas(Filter filterShow)
        {
            if(caseSensitive&&!searchEverywhere)
            {
                return _cryptBlock.GetAll_List().FindAll(i => i.Title.Contains(filterShow.BlockName));
            }
            else if(!caseSensitive && !searchEverywhere)
            {
                return _cryptBlock.GetAll_List().FindAll(i => 
                i.Title.ToLower().Contains(filterShow.BlockName.ToLower()));
            }
            else if (caseSensitive && searchEverywhere)
            {
                return _cryptBlock.GetAll_List().FindAll(i => i.ToString().Contains(filterShow.BlockName));
            }
            else //if (!caseSensitive && searchEverywhere)
            {
                return _cryptBlock.GetAll_List().FindAll(i =>
                i.ToString().ToLower().Contains(filterShow.BlockName.ToLower()));
            }
        }

        public void InitCryptFiles(string key)
        {
            foreach(var path in _appSettings.DirCryptFile)
            {
                if (!System.IO.File.Exists(path.Path))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(path.Path)))
                        Directory.CreateDirectory(Path.GetDirectoryName(path.Path));
                    EncryptDecryptService cryptDecrypt = new EncryptDecryptService(
                        new EncryptDecryptSettings() { Key = key, Path = path.Path });
                    cryptDecrypt.GetInitData();
                    cryptDecrypt.EncryptAndSaveData();
                }
            }
            
        }

        

        
    }
}
