using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ExtraFunction.Utils
{
    public class PasswordHasher
    {

        public static string HashPassword(string plainPassword)
        {
            SHA1CryptoServiceProvider sha1 = new();
            byte[] pwInBytes = Encoding.ASCII.GetBytes(plainPassword);
            byte[] encryptedBytes = sha1.ComputeHash(pwInBytes);
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
