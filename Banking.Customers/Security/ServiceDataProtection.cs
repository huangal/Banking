using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Banking.Customers.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json;

namespace Banking.Customers.Security
{
    public class ServiceDataProtection: IServiceDataProtection
    {
        private readonly IDataProtector _protector;
        public ServiceDataProtection(IDataProtectionProvider dataProtectionProvider, UniqueCode uniqueCode)
        {
            _protector = dataProtectionProvider.CreateProtector(uniqueCode.SecrectKey);
        }

        public class RandomGenerator
        {
            private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

            public static string GenerateString(int length)
            {
                var bytes = new byte[length];

                using (var random = RandomNumberGenerator.Create())
                {
                    random.GetBytes(bytes);
                }

                return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
            }
        }


        public string CalculateHash(string input)
        {
            var salt = GenerateSalt(16);

            var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
        }

        public bool CheckMatch(string hash, string input)
        {
            try
            {
                var parts = hash.Split(':');

                var salt = Convert.FromBase64String(parts[0]);

                var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 16);

                return parts[1].Equals(Convert.ToBase64String(bytes));
            }
            catch
            {
                return false;
            }
        }


        private static byte[] GenerateSalt(int length)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }

        //--------

        public string Encrypt<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            return Encryp(json);
        }

        public string Encryp(string plaintext)
        {
            return _protector.Protect(plaintext);
        }

        public bool TryDecrypt<T>(string encryptedText, out T obj)
        {
            if (TryDecrypt(encryptedText, out var json))
            {
                obj = JsonConvert.DeserializeObject<T>(json);

                return true;
            }

            obj = default(T);

            return false;
        }

        public bool TryDecrypt(string encryptedText, out string decryptedText)
        {
            try
            {
                decryptedText = _protector.Unprotect(encryptedText);

                return true;
            }
            catch (CryptographicException)
            {
                decryptedText = null;

                return false;
            }
        }


        //public string Encrypt(string data)
        //{

        //    return protector.Protect(data);



        //    byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(data);
        //    //string value = Convert.ToBase64String(encodedBytes);
        //    //return protector.Protect(data);

        //    var dataBytes = protector.Protect(encodedBytes);

        //    return Convert.ToBase64String(dataBytes);



        //}
        //public string Decrypt(string data)
        //{
        //    return protector.Unprotect(data);


        //    byte[] dataBytes = Convert.FromBase64String(data);
        //    byte[] value = protector.Unprotect(dataBytes);
        //    return System.Text.Encoding.Unicode.GetString(value);

        //    //var value=  protector.Unprotect(data);
        //    //byte[] decodedBytes = Convert.FromBase64String(value);
        //    //return System.Text.Encoding.Unicode.GetString(decodedBytes);

        //}

    }
}
