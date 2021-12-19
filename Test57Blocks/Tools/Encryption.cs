using System;
using System.Security.Cryptography;
using System.Text;

namespace Test57Blocks.Tools
{
    public static class Encryption
    {
        public static string EncriptString(string value)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] inputBytes = (new UnicodeEncoding()).GetBytes(value);
            byte[] hash = sha1.ComputeHash(inputBytes);
            return Convert.ToBase64String(hash);
        }
    }
}
