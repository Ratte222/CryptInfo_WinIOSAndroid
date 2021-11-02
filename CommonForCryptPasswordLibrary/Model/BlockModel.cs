using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class BlockModel: IEquatable<BlockModel>
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        //public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }        
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string AdditionalInfo { get; set; }
        public string HashSha512 { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DateTimeCreate { get; set; }
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime DateTimeUpdate { get; set; }
        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}\r\n" +
                $"{nameof(Description)}: {Description}\r\n" +
                $"{nameof(UserName)}: {UserName}\r\n" +
                $"{nameof(Email)}: {Email}\r\n" +
                $"{nameof(Password)}: {Password}\r\n" +
                $"{nameof(Phone)}: {Phone}\r\n" +
                $"{nameof(AdditionalInfo)}: {AdditionalInfo}";
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            BlockModel objAsPart = obj as BlockModel;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public bool Equals(BlockModel other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
    }
}
