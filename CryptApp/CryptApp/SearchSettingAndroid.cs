using CommonForCryptPasswordLibrary.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptApp
{
    public class SearchSettingAndroid:ISearchSettings
    {
        [JsonIgnore]
        public readonly string pathToSettings = "nill";
        //public readonly string pathToSettings = System.IO.Path.Combine(
        //    System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "JSON", "SearchSettings.json");

        [JsonProperty(PropertyName = "case_sensitive")]
        public bool CaseSensitive { get; set; } = false;
        [JsonProperty(PropertyName = "search_in_tegs")]
        public bool SearchInTegs { get; set; } = false;
        [JsonProperty(PropertyName = "search_in_header")]
        public bool SearchInHeader { get; set; } = false;
        [JsonProperty(PropertyName = "search_until_first_match")]
        public bool SearchUntilFirstMatch { get; set; } = true;
        [JsonProperty(PropertyName = "view_service_information")]
        public bool ViewServiceInformation { get; set; } = false;
        [JsonProperty(PropertyName = "search_everywhere")]
        public bool SearchEverywhere { get; set; } = false;

        public void Save()
        {
            //base.SerializeToFile(this, pathToSettings);
        }
    }
}
