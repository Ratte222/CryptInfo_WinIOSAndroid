using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.DTO
{
    public class GroupDataDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BlockDataDTO> CryptBlockModels { get; set; } = new List<BlockDataDTO>();

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}\r\n" +
                $"{nameof(Description)}: {Description}";
        }
    }
}
