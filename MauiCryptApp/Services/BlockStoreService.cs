using CommonForCryptPasswordLibrary.Filters;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using CommonForCryptPasswordLibrary.Services;
using MauiCryptApp.Helpers;
using MauiCryptApp.Interfaces;
using MauiCryptApp.Models;

namespace MauiCryptApp.Services
{
    public class BlockStoreService : DataStoreServiceBase<Item>, IDataStore<Item>
    {
        
        List<BlockModel> blocks;
        
        


        protected override void InitData()
        {
            base.InitData();
            if (encryptedFileLoaded)
            {
                blocks = _cryptBlock.GetAll_List();
                items = blocks.Select(x => x.MapToItem()).ToList();
            }
        }

        public Task<IEnumerable<Item>> Search(string search, ISearchSettings searchSettings)
        {
            _inputOutputFile.SetSearchSettings(searchSettings);
            _inputOutputFile.LoadDefaultParams();
            var filter = new Filter()
            {
                BlockName = search
            };
            if (!encryptedFileLoaded)
                return Task.FromResult<IEnumerable<Item>>([]);
            blocks = _inputOutputFile.GetBlockDatas(filter);
            var query = blocks.Select(x => x.MapToItem());
            //if(searchFilter.OrderByLastModifyDate)
            //{
            //    query.OrderBy(x => x.LastModifiedAt);
            //}
            items = query.ToList();
            return Task.FromResult<IEnumerable<Item>>(items);
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            //items.Add(item);
            _cryptBlock.Add(item.MapToBlockModel());
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
        

        
    }
}
