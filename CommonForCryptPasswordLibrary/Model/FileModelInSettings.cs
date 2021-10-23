using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Model
{
    public class FileModelInSettings
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)} = {Name}\r\n" +
                $"{nameof(Path)} = {Path}";
        }
    }
}
