using System;

namespace CryptApp.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string AdditionalInfo { get; set; }
    }
}