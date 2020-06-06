using System;
using System.Text;
using Banking.Customers.Domain.Interfaces;
using Microsoft.AspNetCore.DataProtection;

namespace Banking.Customers.Security
{
    public class ServiceDataProtection: IServiceDataProtection
    {
        private readonly IDataProtector protector;
        public ServiceDataProtection(IDataProtectionProvider dataProtectionProvider, UniqueCode uniqueCode)
        {
            protector = dataProtectionProvider.CreateProtector(uniqueCode.SecrectKey);
        }
        public string Decrypt(string data)
        {
            var value=  protector.Unprotect(data);

            byte[] decodedBytes = Convert.FromBase64String(value);
            return System.Text.Encoding.Unicode.GetString(decodedBytes);

        }
        public string Encrypt(string data)
        {

            byte[] encodedBytes = System.Text.Encoding.Unicode.GetBytes(data);
            string value = Convert.ToBase64String(encodedBytes);

            return protector.Protect(value);
        }
    }
}
