using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace _1._SOVA
{
    public class PasswordService
    {
        private static RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        public static string GenerateSalt(int size)
        {
            var buffer = new byte[size];
            _rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        public static string HashPassword(string pwd, int size)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                pwd,
                Encoding.UTF8.GetBytes("ThisIsAPlaceHolder"),
                KeyDerivationPrf.HMACSHA256,
                10000,
                size));
        }
    }
}
