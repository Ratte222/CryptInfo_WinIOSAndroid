using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class CryptFileModel
    {
        public Dictionary<string, List<CryptBlockModel>> DecryptInfoContent { get; set; }
    }
}
