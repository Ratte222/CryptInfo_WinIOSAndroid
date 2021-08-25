using System;

namespace CryptApp.Models
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