using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ICryptBlock: IRepository<CryptBlockModel>
    {
        List<CryptBlockModel> GetAll_List();
        void LoadData(CryptDecryptSettings settings);
        public bool DataExist { get; }
    }
}
