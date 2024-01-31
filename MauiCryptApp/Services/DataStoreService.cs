using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;

namespace MauiCryptApp.Services
{
    public class DataStoreService : IDataStore<Item>
    {
        List<Item> items;
        List<BlockModel> blocks;
        IMainLogicService _inputOutputFile;
        IBlockService _cryptBlock;
        IGroupService _cryptGroup;
        IEncryptDecryptService _encryptDecryptService;
        //IAppSettings _appSettings;
        //ISearchSettings _searchSettings;
        ApplicationSettings _applicationSettings;
        string key = "";
        private bool cryptedFileLoaded = false; 
        public DataStoreService()
        {
            //_appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IAppSettings>();
            //_searchSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ISearchSettings>();
            _applicationSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ApplicationSettings>();
            //string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt");
            
            _encryptDecryptService = new EncryptDecryptService(new CryptService_Windows());
            
            _cryptBlock = new BlockService(_encryptDecryptService);
            _cryptGroup = new GroupService(_encryptDecryptService);
            var io = new Maui_IO_Service();
            //_inputOutputFile = new MainLogicService(io, _appSettings, _searchSettings, _cryptGroup, _cryptBlock);
            _inputOutputFile = new MainLogicService(io, _applicationSettings.AppSettings, _applicationSettings.SearchSettings, _cryptGroup, _cryptBlock);
            
            items = new List<Item>();
            



        }

        

        public bool SetKey(string key)
        {
            if (!string.IsNullOrEmpty(key)&&!cryptedFileLoaded)
            {
                this.key = key;
                InitData();
                
            }
            return cryptedFileLoaded;
        }
        private void InitData()
        {
            if (items.Count == 0 && !cryptedFileLoaded)
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
                cryptedFileLoaded = true;
                blocks = _cryptBlock.GetAll_List();
                items = blocks.Select(x => x.Map()).ToList();
            }
        }
        public async Task<IEnumerable<Item>> Search(string search)
        {
            var filter = new Filter()
            {
                BlockName = search
            };
            if (!cryptedFileLoaded)
                return new Item[0];
            blocks = _inputOutputFile.GetBlockDatas(filter);
            items = blocks.Select(x => x.Map()).ToList();
            return items;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);
            var block = blocks.First(x=>x.Id == item.Id);
            block.UpdateBlockModel(item);
            _cryptBlock.Update(block);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            return await Task.FromResult(true);
        }
        //ToDo: align all id to Guide data type
        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id.ToString() == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
