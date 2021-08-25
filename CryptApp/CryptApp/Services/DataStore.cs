using CryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonForCryptPasswordLibrary;
using System.IO;
using Xamarin.Forms;

namespace CryptApp.Services
{
    public class DataStore : IDataStore<Item>
    {
        readonly List<Item> items;
        protected SettingAndroid settings;
        protected MyIOAndroid myIOAndroid;
        InputOutputFile inputOutputFile;
        string key = "12345678";
        public DataStore()
        {
            myIOAndroid = new MyIOAndroid();
            settings = new SettingAndroid(myIOAndroid);
            IGetPathToFile getPathToFile = DependencyService.Get<IGetPathToFile>();
            settings.SetDirCryptFile(Path.Combine(getPathToFile.GetPathToCryptFile(), "Crypt.txt"));
            
            inputOutputFile = new InputOutputFile(myIOAndroid, settings);
            if (!File.Exists(settings.GetDirCryptFile()))
            {
                File.Create(settings.GetDirCryptFile());
                inputOutputFile.WriteToEndCryptFile(key,
                    "Example 1 \r\n" +
                    "tegs\r\n" +
                    "e - mail:nik@gmail.com\r\n" +
                    "password: secretPassword\r\n");
            }
            
            items = new List<Item>();
            //{
            //new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }               
            //};
            myIOAndroid.Output = "";
            int[] vs;
            int blockCount = inputOutputFile.GetCountBlock(key);
            if(blockCount >= 1)
            for(int i = 0; i< blockCount; i++)
            {
                string content = inputOutputFile.GetBlockData(out vs, key, i);
                string[] splitContent = content.Split('\n');
                items.Add(new Item
                {
                    Id = Guid.NewGuid().ToString(),
                    Text = splitContent[0].TrimEnd(new char[] { '\r', '\n' }),
                    Description = content.Substring(splitContent[0].Length + 1)
                });                    
            }    
            
            

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