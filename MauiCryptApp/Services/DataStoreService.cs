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
        private bool cryptedFileLoaded = false; 
        public DataStoreService()
        {
            _appSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<IAppSettings>();
            _searchSettings = MauiProgram.ServiceScope.ServiceProvider.GetRequiredService<ISearchSettings>();
            //string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Crypt");
            
            _encryptDecryptService = new EncryptDecryptService(new CryptService_Windows());
            
            _cryptBlock = new BlockService(_encryptDecryptService);
            _cryptGroup = new GroupService(_encryptDecryptService);
            var io = new Maui_IO_Service();
            _inputOutputFile = new MainLogicService(io, _appSettings, _searchSettings, _cryptGroup, _cryptBlock);
            
            items = new List<Item>();
            



        }

        private Item Map(BlockModel blockModel)
        {
            return new Item()
            {
                Id = blockModel.Id,
                Title = blockModel.Title,
                Description = blockModel.Description,
                Email = blockModel.Email,
                Password = blockModel.Password,
                UserName = blockModel.UserName,
                Phone = blockModel.Phone,
                AdditionalInfo = blockModel.AdditionalInfo,
                //Text = blockModel.Title,
                //Description = blockModel.ToString()
            };
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
                    EncryptPath = _appSettings.SelectedCryptFile.Path
                };
                try//for android. exception with calculate sha
                {
                    _encryptDecryptService.LoadData(settings);
                }
                catch (Exception ex)
                {

                }
                cryptedFileLoaded = true;
                foreach (var block in _cryptBlock.GetAll_Enumerable())
                {
                    items.Add(Map(block));
                }
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
