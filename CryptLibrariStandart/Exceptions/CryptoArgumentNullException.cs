using System;
using System.Collections.Generic;
using System.Text;

namespace CryptLibrariStandart.Exceptions
{
    public class CryptoArgumentNullException: Exception
    {
        public CryptoArgumentNullException(string message) : base(message)
        { }
    }
}
