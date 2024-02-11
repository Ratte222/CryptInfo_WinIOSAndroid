using CommonForCryptPasswordLibrary.Model;
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
    public class GroupStoreService : DataStoreServiceBase<Group>, IDataStore<Group>
    {
        List<GroupModel> _groups;
        protected override void InitData()
        {
            base.InitData();
            if (encryptedFileLoaded)
            {
                _groups = _cryptGroup.GetAll_List();
                items = _groups.Select(x => x.MapToGroup()).ToList();
            }
        }

        public Task<bool> AddItemAsync(Group item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Group>> Search(string search)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateItemAsync(Group item)
        {
            throw new NotImplementedException();
        }
    }
}
