using CommonForCryptPasswordLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class CryptFileModel
    {
        public string Hash { get; set; }
        public List<CryptGroupModel> DecryptInfoContent { get; set; } = new List<CryptGroupModel>();

        public void CommonChecksForCryptBlockModel(CryptBlockModel item)
        {
            if (item == null)
                throw new ValidationException($"{nameof(item)} is null");
            if (String.IsNullOrEmpty(item.Title))
                throw new ValidationException($"{nameof(item.Title)} is null or empty");
            if (item.GroupId == null)
                throw new ValidationException($"{nameof(item.GroupId)} is null");
        }
    }
}
