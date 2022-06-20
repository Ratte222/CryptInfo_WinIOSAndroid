using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Models
{
    public class Item
    {
        public string Id { get; set; }
        public int StartBlockLine { get; set; }
        public int EndBlockLine { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}
