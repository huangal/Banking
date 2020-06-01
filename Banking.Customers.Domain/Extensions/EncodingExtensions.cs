using System;
using System.IO;
using System.Text;
using System.Web;

namespace Banking.Customers.Domain.Extensions
{
    public static class EncodingExtensions
    {
        public static string Base64Encode(this string value)
        {
            return Base64Encode(value, Encoding.UTF8);
        }

        public static string Base64Encode(this string value, Encoding encoder)
        {
            if (!value.IsValidString()) return null;
            var plainTextBytes = encoder.GetBytes(value);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string value)
        {
            return Base64Decode(value, Encoding.UTF8);
        }

        public static string Base64Decode(this string value, Encoding encoder)
        {
            if (!value.IsValidString()) return null;
            var base64EncodedBytes = Convert.FromBase64String(value);
            return encoder.GetString(base64EncodedBytes);
        }

        public static bool IsValidString(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        public static string UrlEncode(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? HttpUtility.UrlEncode(value) : string.Empty;
        }

        public static string UrlDecode(this string value)
        {
            return !string.IsNullOrWhiteSpace(value) ? HttpUtility.UrlDecode(value) : string.Empty;
        }


        public static byte[] ConvertToBytes(this string value)
        {
            if (!value.IsValidString()) return null;
            return Encoding.UTF8.GetBytes(value);
        }

        public static byte[] ConvertToBytes(this Stream value)
        {
            if (value == null) return null;

            using (var memoryStream = new MemoryStream())
            {
                value.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static byte[] ToBytesFromBase64(this string value)
        {
            if (!value.IsValidString()) return null;
            return Convert.FromBase64String(value);
        }


        public static string ConvertToString(this byte[] value)
        {
            if (value == null) return null;
            return Encoding.UTF8.GetString(value);
        }

        public static string ToStringBase64(this byte[] value)
        {
            if (value == null) return null;
            return Convert.ToBase64String(value);
        }



    }
}
