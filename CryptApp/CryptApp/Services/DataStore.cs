using CryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonForCryptPasswordLibrary;
using System.IO;
using Xamarin.Forms;
using CommonForCryptPasswordLibrary.Services;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Exceptions;

namespace CryptApp.Services
{
    public class DataStore : IDataStore<Item>
    {
        readonly List<Item> items;
        protected SettingAndroid settings;
        protected MyIOAndroid myIOAndroid;
        IMainLogicService inputOutputFile;
        string key = "";
        public DataStore()
        {
            myIOAndroid = new MyIOAndroid();
            settings = new SettingAndroid();
            CryptApp.Helpers.AppSettings appSettings =  DependencyService.Get
                <CryptApp.Helpers.AppSettings>(DependencyFetchTarget.GlobalInstance);
            key = appSettings.Password;
            IGetPathToFile getPathToFile = DependencyService.Get<IGetPathToFile>();
            if (appSettings.TestMode)
            {
                settings.DirCryptFile.Add(new FileModelInSettings()
                {
                    Name = "Test",
                    Path = Path.Combine(getPathToFile.GetPathToCryptFile(), "CryptTestV2.txt")
                });
                settings.selected_crypr_file = "Test";
            }
            else
            {
                settings.DirCryptFile.Add(new FileModelInSettings()
                {
                    Name = "Work",
                    Path = Path.Combine(getPathToFile.GetPathToCryptFile(), "Crypt")
                });
                settings.selected_crypr_file = "Work";
            }
            SearchSettingAndroid searchSettingAndroid = new SearchSettingAndroid();
            IEncryptDecryptService encryptDecryptService = new EncryptDecryptService();
            IGroupService groupService = new GroupService(encryptDecryptService);
            IBlockService blockService = new BlockService(encryptDecryptService);
            inputOutputFile = new MainLogicService(myIOAndroid, settings, searchSettingAndroid,
                groupService, blockService);
            if (!File.Exists(settings.SelectedCryptFile.Path))
            {
                inputOutputFile.InitCryptFile(key);
            }
            if (!blockService.DataExist)
            {
                try
                {
                    encryptDecryptService.LoadData(
                  new EncryptDecryptSettings()
                  {
                      Path = settings.SelectedCryptFile.Path,
                      Key = key
                  });
                }
                catch(TheFileIsDamagedException ex)
                {

                }
                catch(Exception ex)
                {

                }
                
            }
            items = new List<Item>();
            
            myIOAndroid.Output = "";
            try
            {
                foreach(var block in blockService.GetAll_List())
                {
                    items.Add(new Item()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = block.Title,
                        Description = block.Description,
                        UserName = block.UserName,
                        Email = block.Email,
                        Password = block.Password,
                        Phone = block.Phone,
                        AdditionalInfo = block.AdditionalInfo
                    });
                }
                //int[] vs;
                //int blockCount = inputOutputFile.GetCountBlock(key);
                //if (blockCount >= 1)
                //    for (int i = 0; i < blockCount; i++)
                //    {
                //        try
                //        {
                //            string content = inputOutputFile.GetBlockData(out vs, key, i);
                //            string[] splitContent = content.Split('\n');
                //            items.Add(new Item
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                StartBlockLine = vs[0],
                //                EndBlockLine = vs[1],
                //                Text = splitContent[0].TrimEnd(new char[] { '\r', '\n' }),
                //                Description = content.Substring(splitContent[0].Length + 1)
                //            });
                //        }
                //        catch(Exception ex)
                //        {
                //            items.Add(new Item
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                Text = $"Block number {i}",
                //                Description = $"message: {ex.Message}\r\n" +
                //                $"InnerException: {ex?.InnerException}" +
                //                $"StackTrace: {ex?.StackTrace}" +
                //                $"HResult: {ex?.HResult}"
                //            });
                //        }
                        
                //    }
            }
            catch(Exception ex)
            {

            }
            items = items.OrderBy(i => i.Title).ToList();
            

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