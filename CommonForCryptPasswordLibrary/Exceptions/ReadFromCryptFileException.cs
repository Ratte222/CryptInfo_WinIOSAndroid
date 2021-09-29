using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Exceptions
{
    public class ReadFromCryptFileException: Exception
    {
        public string Property { get; protected set; }
        public ReadFromCryptFileException(string message, string prop = "") : base(message)
        {
            Property = prop;
        }
    }
}
