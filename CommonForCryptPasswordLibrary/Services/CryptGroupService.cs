﻿using CommonForCryptPasswordLibrary.Exceptions;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonForCryptPasswordLibrary.Services
{
    public class CryptGroupService : ICryptGroup
    {
        private ICryptDecrypt _cryptDecrypt;
        public bool DataExist
        {
            get
            {
                return _cryptDecrypt.CryptFileModel != null;
            }
        }
        public CryptGroupService(ICryptDecrypt cryptDecrypt)
        {
            _cryptDecrypt = cryptDecrypt;
        }

        public List<CryptGroupModel> GetAll_List()
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent;
        }

        public IEnumerable<CryptGroupModel> GetAll()
        {
            return _cryptDecrypt.CryptFileModel.DecryptInfoContent.AsEnumerable();
        }

        //public CryptGroupModel Get(Guid guid)
        public CryptGroupModel Get(Predicate<CryptGroupModel> predicate)
        {
            //return _cryptFileModel.DecryptInfoContent.Find(i => i.Id == guid);
            throw new NotImplementedException();
        }

        public void Add(CryptGroupModel item)
        {
            if (String.IsNullOrEmpty(item.Name))
                throw new ValidationException($"{nameof(item.Name)} is null or empty");
            if(_cryptDecrypt.CryptFileModel.DecryptInfoContent.Any(i=>i.Name.ToLower() == item.Name.ToLower()))
                throw new ValidationException($"This group name already exist");
            if (item.Id == null)
                item.Id = new Guid();
            _cryptDecrypt.CryptFileModel.DecryptInfoContent.Add(item);
        }

        public void Delete(CryptGroupModel item)
        {
            if (item == null)
                throw new ValidationException($"{nameof(item)} is null");
            _cryptDecrypt.CryptFileModel.DecryptInfoContent.Remove(item);
        }

        public void Edit(CryptGroupModel item)
        {
            CryptGroupModel temp = _cryptDecrypt.CryptFileModel
                .DecryptInfoContent.FirstOrDefault(i => i.Id == item.Id);
            if (temp == null)
                throw new ValidationException("No element with this id");
            temp.Name = item.Name;
            temp.Description = item.Description;
        }

        

        public void LoadData(CryptDecryptSettings settings)
        {
            _cryptDecrypt.LoadData(settings);
        }
    }
}
