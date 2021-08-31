using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCrypt.DTO
{
    public class BlockDataDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string AdditionalInfo { get; set; }

        public override string ToString()
        {
            return $"{nameof(Title)}: {Title}\r\n" +
                $"{nameof(Description)}: {Description}\r\n" +
                $"{nameof(UserName)}: {UserName}\r\n" +
                $"{nameof(Email)}: {Email}\r\n" +
                $"{nameof(Password)}: {Password}\r\n" +
                $"{nameof(Phone)}: {Phone}\r\n" +
                $"{nameof(AdditionalInfo)}: {AdditionalInfo}\r\n";
        }
    }
}
