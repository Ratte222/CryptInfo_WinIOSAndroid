using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    public class DataStoreServiceBase<T> where T : BaseDataModel
    {
        protected List<T> items;
        protected IMainLogicService _inputOutputFile;
        protected IBlockService _cryptBlock;
        protected IGroupService _cryptGroup;
        protected IEncryptDecryptService _encryptDecryptService;
        protected ApplicationSettings _applicationSettings;
        protected string key = "";
        protected private bool encryptedFileLoaded = false;
        public DataStoreServiceBase()
        {
            _applicationSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ApplicationSettings>();
            //string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt");

            _encryptDecryptService = new EncryptDecryptService(new CryptService_Windows());

            _cryptBlock = new BlockService(_encryptDecryptService);
            _cryptGroup = new GroupService(_encryptDecryptService);
            var io = new Maui_IO_Service();
            //_inputOutputFile = new MainLogicService(io, _appSettings, _searchSettings, _cryptGroup, _cryptBlock);
            _inputOutputFile = new MainLogicService(io, _applicationSettings.AppSettings, _applicationSettings.SearchSettings, _cryptGroup, _cryptBlock);
            items = new List<T>();
        }

        public bool SetKey(string key)
        {
            this.key = key;
            InitData();
            return encryptedFileLoaded;
        }

        protected virtual void InitData()
        {
            var settings = new EncryptDecryptSettings()
            {
                Key = key,
                EncryptPath = _applicationSettings.AppSettings.SelectedCryptFile.Path
            };
            try//for android. exception with calculate sha
            {
                _encryptDecryptService.LoadData(settings);
            }
            catch (Exception ex)
            {

            }
            encryptedFileLoaded = true;    
        }

        public virtual async Task<T> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public virtual async Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
