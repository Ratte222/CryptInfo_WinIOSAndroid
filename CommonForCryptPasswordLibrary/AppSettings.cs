using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary
{
    [Serializable]
    public class AppSettings
    {
        public string DirCryptFile { get; set; }
        public string DirDecryptFile { get; set; }
        public string SeparateBlock { get; set; }
        public string CharStartAttributes { get; set; }
        public string CharStopAttributes { get; set; }
        public SearchSettingDefault SearchSettingDefault;
    }
    [Serializable]
    public class SearchSettingDefault
    {
        public bool caseSensitive { get; set; }
        public bool searchInTegs { get; set; }
        public bool searchInHeader { get; set; }
        public bool searchUntilFirstMatch { get; set; }
        public bool viewServiceInformation { get; set; }
    }
}
