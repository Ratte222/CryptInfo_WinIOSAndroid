using CommonForCryptPasswordLibrary.Exceptions;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonForCryptPasswordLibrary.Services
{
    public class GroupService : IGroupService
    {
        private IEncryptDecryptService _cryptDecrypt;
        public bool DataExist
        {
            get
            {
                return _cryptDecrypt.CryptFileModel != null;
            }
        }
        public GroupService(IEncryptDecryptService cryptDecrypt)
        {
            _cryptDecrypt = cryptDecrypt;
        }

        public List<GroupModel> GetAll_List()
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent;
        }

        public IEnumerable<GroupModel> GetAll_Enumerable()
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent.AsEnumerable();
        }

        public IQueryable<GroupModel> GetAll_Queryable()
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent.AsQueryable();
        }

        //public CryptGroupModel Get(Guid guid)
        public GroupModel Get(Predicate<GroupModel> predicate)
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent.Find(predicate);            
        }

        public void Add(GroupModel item)
        {
            if (String.IsNullOrEmpty(item.Name))
                throw new ValidationException($"{nameof(item.Name)} is null or empty");
            if(_cryptDecrypt.CryptFileModel.DecryptInfoContent.Any(i=>i.Name.ToLower() == item.Name.ToLower()))
                throw new ValidationException($"This group name already exist");
            item.Id = Guid.NewGuid();
            _cryptDecrypt.CryptFileModel.DecryptInfoContent.Add(item);
            _cryptDecrypt.SaveChanges();
        }

        public void Delete(GroupModel item)
        {
            if (item == null)
                throw new ValidationException($"{nameof(item)} is null");
            _cryptDecrypt.CryptFileModel.DecryptInfoContent.Remove(item);
            _cryptDecrypt.SaveChanges();
        }

        public void Update(GroupModel item)
        {
            GroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i => i.Id == item.Id);
            if (temp == null)
                throw new ValidationException("No element with this id");
            temp.Name = item.Name;
            temp.Description = item.Description;
            _cryptDecrypt.SaveChanges();
        }

        

        public void LoadData(EncryptDecryptSettings settings)
        {
            _cryptDecrypt.LoadData(settings);
        }
    }
}
