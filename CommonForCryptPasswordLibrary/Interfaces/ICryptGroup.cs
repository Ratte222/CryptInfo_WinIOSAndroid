using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ICryptGroup : IRepository<CryptGroupModel>
    {
        List<CryptGroupModel> GetAll_List();
        void LoadData(CryptDecryptSettings settings);
        public bool DataExist { get; }
    }
}
