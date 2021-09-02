using CommonForCryptPasswordLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface IBlockService: IRepository<BlockModel>
    {
        List<BlockModel> GetAll_List();
        void LoadData(EncryptDecryptSettings settings);
        public bool DataExist { get; }
    }
}
