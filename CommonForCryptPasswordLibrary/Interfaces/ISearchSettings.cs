using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ISearchSettings
    {
        bool CaseSensitive { get; set; }
        bool SearchInTegs { get; set; }
        bool SearchInHeader { get; set; }
        bool SearchUntilFirstMatch { get; set; }
        bool ViewServiceInformation { get; set; }
        void Save();
    }
}
