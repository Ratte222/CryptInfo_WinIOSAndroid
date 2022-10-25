using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCryptApp.Models
{
    public record LogModel
    {
        public string Level { get; set; }
        public string Description { get; set; }
        public LogModel()
        {

        }
        public LogModel(string description, string level = null)
        {
            Description = description;
            Level = level;
        }
    }
}
