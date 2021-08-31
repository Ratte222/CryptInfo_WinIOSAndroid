using CommonForCryptPasswordLibrary.Exceptions;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonForCryptPasswordLibrary.Services
{
    public class CryptBlockService:ICryptBlock
    {
        private ICryptDecrypt _cryptDecrypt;
        public bool DataExist
        {
            get
            {
                return _cryptDecrypt.CryptFileModel != null;
            }
        }
        public CryptBlockService(ICryptDecrypt cryptDecrypt)
        {
            _cryptDecrypt = cryptDecrypt;
        }

        

        public CryptBlockModel Get(Predicate<CryptBlockModel> predicate)
        {
            //CryptBlockModel result = null;
            //foreach(var value in _cryptFileModel.DecryptInfoContent)
            //{
            //    result = value.CryptBlockModels.FirstOrDefault(predicate);
            //}
            //return result;
            throw new NotImplementedException();
        }

        public IEnumerable<CryptBlockModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<CryptBlockModel> GetAll_List()
        {
            List<CryptBlockModel> cryptBlockModels = new List<CryptBlockModel>();
            foreach(var value in _cryptDecrypt.CryptFileModel.DecryptInfoContent)
            {
                cryptBlockModels.AddRange(value.CryptBlockModels);
            }            
            return cryptBlockModels;
        }

        public void Add(CryptBlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            if (_cryptDecrypt.CryptFileModel.DecryptInfoContent.Any(i => i.CryptBlockModels
                .Any(j => j.Title.ToLower() == item.Title.ToLower()) ))
                throw new ValidationException($"This block name already exist");            
            if (item.Id == null)
                item.Id = new Guid();            
            CryptGroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i=>i.Id == item.GroupId);
            if(temp == null)
                throw new ValidationException($"There is no group in this {nameof(item.GroupId)}");
            temp.CryptBlockModels.Add(item);
        }

        public void Delete(CryptBlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            CryptGroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i => i.Id == item.GroupId);
            if (temp == null)
                throw new ValidationException($"There is no group in this {nameof(item.GroupId)}");
            temp.CryptBlockModels.Remove(item);
        }

        public void Edit(CryptBlockModel item)
        {
            _cryptDecrypt.CryptFileModel.CommonChecksForCryptBlockModel(item);
            CryptBlockModel temp = _cryptDecrypt.CryptFileModel
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
        }

        

        public void LoadData(CryptDecryptSettings settings)
        {
            _cryptDecrypt.LoadData(settings);
        }
    }
}
