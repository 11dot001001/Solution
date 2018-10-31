using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using GameCore;

namespace Database.Data.Model.Tools
{
    public static class AccountTools
    {
        private static readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        private static readonly SHA512 sha512 = SHA512.Create();

        private static readonly int passwordSaltLength = AccountSetting.minPasswordLength * 2;
        private static readonly int passwordHashLength = 64;

        private static byte[] GetPasswordData(string password)
        {
            byte[] salt = new byte[passwordSaltLength];
            rng.GetBytes(salt);
            byte[] hash = GetPasswordHash(salt, password);
            byte[] result = new byte[passwordSaltLength + passwordHashLength];
            Array.Copy(salt, result, passwordSaltLength);
            Array.Copy(hash, 0, result, passwordSaltLength, passwordHashLength);
            return result;
        }
        private static byte[] GetPasswordHash(byte[] salt, string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            int saltOffset = salt[0] % passwordSaltLength;
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= salt[saltOffset];
                if (saltOffset == passwordSaltLength)
                    saltOffset = 0;
            }
            int hashIterateCount = 128 + salt[1] % 128;
            for (int i = 0; i < hashIterateCount; i++)
                bytes = sha512.ComputeHash(bytes);
            return bytes;
        }

        public static byte[] GetDataPassword(string password)
        {
            return GetPasswordData(password);
        }

        public static bool IsPasswordVerification(Account account, string password)
        {
            byte[] passwordSalt = new byte[passwordSaltLength];
            byte[] passwordHash = new byte[passwordHashLength];

            Array.Copy(account.Password, passwordSalt, passwordSaltLength);

            byte[] pass = GetPasswordHash(passwordSalt, password);

            for (int i = 0; i < passwordHashLength; i++)
                if (!Equals(pass[i], account.Password[passwordSaltLength + i]))
                    return false;
            return true;
        }

        public static int GetPasswordLength()
        {
            return passwordSaltLength + passwordHashLength;
        }
    }
}
