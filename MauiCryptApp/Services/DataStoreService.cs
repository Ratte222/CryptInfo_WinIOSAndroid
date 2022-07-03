using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Services
{
    public class DataStoreService : IDataStore<Item>
    {
        List<Item> items;
        IMainLogicService _inputOutputFile;
        IBlockService _cryptBlock;
        IGroupService _cryptGroup;
        IEncryptDecryptService _encryptDecryptService;
        IAppSettings _appSettings;
        ISearchSettings _searchSettings;
        string key = "";
        public DataStoreService()
        {
            _appSettings = DependencyService.Get<IAppSettings>();
            _searchSettings = DependencyService.Get<ISearchSettings>();
            //string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt");
            
            _encryptDecryptService = new EncryptDecryptService();
            
            _cryptBlock = new BlockService(_encryptDecryptService);
            _cryptGroup = new GroupService(_encryptDecryptService);
            var io = new Maui_IO_Service();
            _inputOutputFile = new MainLogicService(io, _appSettings, _searchSettings, _cryptGroup, _cryptBlock);
            
            //myIOAndroid = new MyIOAndroid();
            //settings = new SettingAndroid(myIOAndroid);
            //Helpers.AppSettings appSettings = DependencyService.Get
            //    <Helpers.AppSettings>(DependencyFetchTarget.GlobalInstance);
            //key = appSettings.Key;
            //IGetPathToFile getPathToFile = DependencyService.Get<IGetPathToFile>();
            //if (appSettings.TestMode)
            //    settings.SetDirCryptFile(Path.Combine(getPathToFile.GetPathToCryptFile(), "CryptTest.txt"));
            //else
            //    settings.SetDirCryptFile(Path.Combine(getPathToFile.GetPathToCryptFile(), "Crypt.txt"));
            //inputOutputFile = new InputOutputFile(myIOAndroid, settings);
            //if (!File.Exists(settings.GetDirCryptFile()))
            //{
            //    File.Create(settings.GetDirCryptFile());
            //    inputOutputFile.WriteToEndCryptFile(key,
            //        "Example 1 \r\n" +
            //        "tegs\r\n" +
            //        "e - mail:nik@gmail.com\r\n" +
            //        "password: secretPassword\r\n");
            //}

            items = new List<Item>(
                //new[]{
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description = "This is an item description." },
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description = "This is an item description." },
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description = "This is an item description." },
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description = "This is an item description." },
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description = "This is an item description." },
                //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description = "This is an item description." }
                //}
                );
            



        }

        private Item Map(BlockModel blockModel)
        {
            return new Item()
            {
                Id = blockModel.Id.ToString(),
                Text = blockModel.Title,
                Description = blockModel.ToString()
            };
        }

        public void SetKey(string key)
        {
            this.key = key;
            InitData();
        }
        private void InitData()
        {
            var settings = new CommonForCryptPasswordLibrary.Model.EncryptDecryptSettings()
            {
                Key = key,
                EncryptPath = _appSettings.SelectedCryptFile.Path
            };
            _encryptDecryptService.LoadData(settings);
            foreach (var block in _cryptBlock.GetAll_Enumerable())
            {
                items.Add(Map(block));
            }
        }
        public async Task<IEnumerable<Item>> Search(string search)
        {
            var filter = new Filter()
            {
                BlockName = search
            };
            items = new List<Item>();
            foreach(var item in _inputOutputFile.GetBlockDatas(filter))
            {
                items.Add(Map(item));
            }
            return items;
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}
