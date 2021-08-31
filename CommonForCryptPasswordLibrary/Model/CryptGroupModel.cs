using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class CryptGroupModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CryptBlockModel> CryptBlockModels { get; set; } = new List<CryptBlockModel>();

        
    }
}
