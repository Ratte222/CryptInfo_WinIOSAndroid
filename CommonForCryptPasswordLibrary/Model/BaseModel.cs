using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonForCryptPasswordLibrary.Model
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public string HashSha512 { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DateTimeCreate { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DateTimeUpdate { get; set; }
    }
}
