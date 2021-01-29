using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt
{
    [Serializable]
    public class AppSettings
    {
        public string DirCryptFile { get; set; }
        public string DirDecryptFile { get; set; }
        public string SeparateBlock { get; set; }
        public string CharStartAttributes { get; set; }
        public string CharStopAttributes { get; set; }
    }
}
