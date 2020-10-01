using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace CryptWinIOSAndroid
{
    [Serializable]
    public class CPSetting
    {
        public string FileNamePassword { get; set; }
        public string Conf { get; set; }
        public string ConfImportant { get; set; }
        [NonSerialized] private string Key;
        [NonSerialized] private string ImportantKey;
        public string GetKey()
        { return Key; }
        public void SetKey(string _key)
        { Key = _key; }
        public string GetImportantKey()
        { return ImportantKey; }
        public void SetImportantKey(string _ImportantKey)
        { ImportantKey = _ImportantKey; }
    }
}
