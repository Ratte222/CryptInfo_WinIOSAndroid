using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CommonForCryptPasswordLibrary.Interfaces;
using AuxiliaryLib.WorkWithJson;
using System.Reflection;

namespace ConsoleCrypt.Helpers
{
    public class SearchSettings : SerializeDeserializeJson<SearchSettings>, ISearchSettings
    {
        //public delegate void SaveSettingData();
        //public event SaveSettingData SaveData;
        //Assembly.GetExecutingAssembly().Location in net5 and letter returned null!!!!!!
        [JsonIgnore]
        public readonly string pathToSettings = System.IO.Path.Combine(
            System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JSON", "SearchSettings.json");

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
        [JsonProperty(PropertyName = "search_everywhere")]
        public bool SearchEverywhere { get; set; }

        public void Save()
        {
            base.SerializeToFile(this, pathToSettings);
        }
    }
}
