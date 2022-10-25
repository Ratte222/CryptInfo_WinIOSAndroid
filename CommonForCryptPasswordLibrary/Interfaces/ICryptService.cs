using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonForCryptPasswordLibrary.Interfaces
{
    public interface ICryptService
    {
        string Encrypt(string content, string key);
        string Decrypt(string content, string key);
        string GetHashSHA512(string content);
    }
}
