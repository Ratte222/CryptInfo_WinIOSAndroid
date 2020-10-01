using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptWinIOSAndroid.Models;
using Xamarin.Forms;

namespace CryptWinIOSAndroid.Services
{
    public class VirtualDataStore : IDataStoreMy<BlockOfInformation>
    {
        //readonly List<Item> items;
        readonly Service MainService;
        public VirtualDataStore()
        {
            MainService = new Service();
            //MainService.DecryptFile();
            
            //items = new List<Item>()
            //{
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "First item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Second item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Third item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fourth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
            //    new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." }
            //};
        }
        
        public async Task<bool> SetKeys(string key, string importantKey)
        {
            MainService.SetKeys(key, importantKey);
            return await Task.FromResult(true);
        }

        public async Task<bool> AddItemAsync(BlockOfInformation item)
        {
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(BlockOfInformation item)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            //items.Remove(oldItem);
            //items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            //var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            //items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<BlockOfInformation> GetItemAsync(string id)
        {
            //return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
            return await Task.FromResult(MainService.blockOfInformations.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<BlockOfInformation>> GetItemsAsync(bool forceRefresh = false)
        {
            //List<Item> items = new List<Item>();
            //for(int i = 0; i < MainService.blockOfInformations.Count; i++)
            //{
            //    items.Add(MainService.blockOfInformations[i]);
            //}
            return await Task.FromResult(MainService.blockOfInformations);
        }
    }
}