using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AvantCredit.Uploader.Core.Security
{
    public class PasswordUtil
    {
        private static Encoding CryptoEncoding
        {
            get { return Encoding.UTF8; }
        }

        private static HashAlgorithm CryptoHashing
        {
            get { return new SHA256Managed(); }
        }

        private static IEnumerable<byte> GetBytes(string value)
        {
            return CryptoEncoding.GetBytes(value);
        }

        private static string GetString(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        private static byte[] SaltPassword(string password, string salt)
        {
            return GetBytes(password).Concat(GetBytes(salt)).ToArray();
        }

        public static string GenerateSalt(int strength = 16)
        {
            //Create and populate random byte array
            var randomArray = new byte[strength];

            //Create random salt and convert to string
            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(randomArray);

            return GetString(randomArray);
        }

        public static string Encrypt(string password, string salt)
        {
            return GetString(CryptoHashing.ComputeHash(SaltPassword(password, salt)));
        }

        public static bool IsMatch(string password, string storedPassword, string salt)
        {
            return storedPassword == Encrypt(password, salt);
        }
    }
}