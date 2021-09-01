using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class CryptGroupModel: IEquatable<CryptGroupModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CryptBlockModel> CryptBlockModels { get; set; } = new List<CryptBlockModel>();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}\r\n" +
                $"{nameof(Description)}: {Description}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            CryptGroupModel objAsPart = obj as CryptGroupModel;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public bool Equals(CryptGroupModel other)
        {
            if (other == null) return false;
            return (this.Id.Equals(other.Id));
        }
    }
}
