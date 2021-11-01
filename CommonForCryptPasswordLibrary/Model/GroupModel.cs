using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class GroupModel: IEquatable<GroupModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime DateTimeUpdate { get; set; }
        public string HashSha512 { get; set; }
        public List<BlockModel> CryptBlockModels { get; set; } = new List<BlockModel>();
        
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}\r\n" +
                $"{nameof(Description)}: {Description}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            GroupModel objAsPart = obj as GroupModel;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public bool Equals(GroupModel other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
    }
}
