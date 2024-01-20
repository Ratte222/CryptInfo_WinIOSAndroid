using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Models
{
    public class Item
    {
        //public string Id { get; set; }
        public int StartBlockLine { get; set; }
        public int EndBlockLine { get; set; }
        //public string Text { get; set; }
        //public string Description { get; set; }
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
    }
}
