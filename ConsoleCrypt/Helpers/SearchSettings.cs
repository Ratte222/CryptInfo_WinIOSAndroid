using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CommonForCryptPasswordLibrary.Interfaces;
using CommonForCryptPasswordLibrary.WorkWithJson;

namespace ConsoleCrypt.Helpers
{
    public class SearchSettings : SerializeDeserializeJson<SearchSettings>, ISearchSettings
    {
        //public delegate void SaveSettingData();
        //public event SaveSettingData SaveData;
        [JsonIgnore]
        public readonly string pathToSettings = System.IO.Path.Combine("JSON", "SearchSettings.json");

        [JsonProperty(PropertyName = "case_sensitive")]
        public bool CaseSensitive { get; set; }
        [JsonProperty(PropertyName = "search_in_tegs")]
        public bool SearchInTegs { get; set; }
        [JsonProperty(PropertyName = "search_in_header")]
        public bool SearchInHeader { get; set; }
        [JsonProperty(PropertyName = "search_until_first_match")]
        public bool SearchUntilFirstMatch { get; set; }
        [JsonProperty(PropertyName = "view_service_information")]
        public bool ViewServiceInformation { get; set; }

        public void Save()
        {
            base.SerializeToFile(this, pathToSettings);
        }
    }
}
