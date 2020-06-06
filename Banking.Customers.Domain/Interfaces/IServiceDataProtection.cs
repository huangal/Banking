using System;
namespace Banking.Customers.Domain.Interfaces
{
    public interface IServiceDataProtection
    {
        string Decrypt(string data);
        string Encrypt(string data);
    }
}
