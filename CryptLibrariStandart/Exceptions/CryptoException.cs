using System;
using System.Collections.Generic;
using System.Text;

namespace CryptLibrariStandart.Exceptions
{
    public class CryptoException:Exception
    {
        public CryptoException(string message):base(message)
        {

        }
    }
}
