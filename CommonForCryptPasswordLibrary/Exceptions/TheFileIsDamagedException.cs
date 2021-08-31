using System;
using System.Collections.Generic;
using System.Text;

namespace CommonForCryptPasswordLibrary.Exceptions
{
    public class TheFileIsDamagedException:Exception
    {
        public string Property { get; protected set; }
        public TheFileIsDamagedException(string message, string prop = "") : base(message)
        {
            Property = prop;
        }
    }
}
