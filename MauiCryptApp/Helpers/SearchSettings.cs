using CommonForCryptPasswordLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Helpers
{
    public class SearchSettings:ISearchSettings
    {
        public bool CaseSensitive { get; set; }
        public bool SearchInTegs { get; set; }
        public bool SearchInHeader { get; set; }
        public bool SearchUntilFirstMatch { get; set; }
        public bool ViewServiceInformation { get; set; }
        public bool SearchEverywhere { get; set; }
        public void Save() { }
    }
}
