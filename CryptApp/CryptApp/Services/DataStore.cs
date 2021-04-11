using CryptApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonForCryptPasswordLibrary;
using System.IO;

namespace CryptApp.Services
{
    public class DataStore : IDataStore<Item>
    {
        readonly List<Item> items;
        protected Settings settings;
        protected MyIOAndroid myIOAndroid;
        C_InputOutputFile inputOutputFile;
        string key = "12345678";
        public DataStore()
        {
            myIOAndroid = new MyIOAndroid();
            settings = new Settings(myIOAndroid, true);
            settings.SetDirCryptFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Crypt.txt"));
            
            inputOutputFile = new C_InputOutputFile(myIOAndroid, settings);
            if (!File.Exists(settings.GetDirCryptFile()))
            {
                File.Create(settings.GetDirCryptFile());
                
            }
            inputOutputFile.WriteToEndCryptFile(key,
                    "Example 1 \r\n" +
                    "tegs\r\n" +
                    "e - mail:12345678\r\n" +
                    "password: password1\r\n");
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
            //string blockData = inputOutputFile.GetBlockData(out vs, "12345678", 0);
            items.Add(new Item
            {
                Id = Guid.NewGuid().ToString(),
                Text = "First item",
                Description = inputOutputFile.GetBlockData(out vs, key, 0)
            });
            items.Add(new Item
            {
                Id = Guid.NewGuid().ToString(),
                Text = "Second item",
                Description = inputOutputFile.GetBlockData(out vs, key, 1)
            });
            items.Add(new Item
            {
                Id = Guid.NewGuid().ToString(),
                Text = "Third item",
                Description = inputOutputFile.GetBlockData(out vs, key, 2)
            });

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
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

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