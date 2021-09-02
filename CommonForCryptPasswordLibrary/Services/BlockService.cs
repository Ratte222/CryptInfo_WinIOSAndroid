using CommonForCryptPasswordLibrary.Exceptions;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonForCryptPasswordLibrary.Services
{
    public class BlockService:IBlockService
    {
        private IEncryptDecryptService _cryptDecrypt;
        public bool DataExist
        {
            get
            {
                return _cryptDecrypt.CryptFileModel != null;
            }
        }
        public BlockService(IEncryptDecryptService cryptDecrypt)
        {
            _cryptDecrypt = cryptDecrypt;
        }

        

        public BlockModel Get(Predicate<BlockModel> predicate)
        {
            return GetAll_List().Find(predicate);
        }

        public IEnumerable<BlockModel> GetAll_Enumerable()
        {
            return GetAll_List().AsEnumerable();
        }

        public IQueryable<BlockModel> GetAll_Queryable()
        {
            return GetAll_List().AsQueryable();
        }

        public List<BlockModel> GetAll_List()
        {
            List<BlockModel> cryptBlockModels = new List<BlockModel>();
            foreach(var value in _cryptDecrypt.CryptFileModel.DecryptInfoContent)
            {
                cryptBlockModels.AddRange(value.CryptBlockModels);
            }            
            return cryptBlockModels;
        }

        public void Add(BlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            if (_cryptDecrypt.CryptFileModel.DecryptInfoContent.Any(i => i.CryptBlockModels
                .Any(j => j.Title.ToLower() == item.Title.ToLower()) ))
                throw new ValidationException($"This block name already exist");            
            item.Id = Guid.NewGuid();            
            GroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i=>i.Id == item.GroupId);
            if(temp == null)
                throw new ValidationException($"There is no group in this {nameof(item.GroupId)}");
            temp.CryptBlockModels.Add(item);
            _cryptDecrypt.SaveChanges();
        }

        public void Delete(BlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            GroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i => i.Id == item.GroupId);
            if (temp == null)
                throw new ValidationException($"There is no group in this {nameof(item.GroupId)}");
            temp.CryptBlockModels.Remove(item);
            _cryptDecrypt.SaveChanges();
        }

        public void Update(BlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            BlockModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i => i.Id == item.GroupId)
                .CryptBlockModels.FirstOrDefault(j => j.Id == item.Id);
            if (temp == null)
                throw new ValidationException("No element with this id");
            temp.Title = item.Title; 
            temp.UserName = item.UserName; 
            temp.Email = item.Email; 
            temp.Password = item.Password; 
            temp.Phone = item.Phone; 
            temp.Description = item.Description; 
            temp.AdditionalInfo = item.AdditionalInfo;
            _cryptDecrypt.SaveChanges();
        }

        

        public void LoadData(EncryptDecryptSettings settings)
        {
            _cryptDecrypt.LoadData(settings);
        }
    }
}
