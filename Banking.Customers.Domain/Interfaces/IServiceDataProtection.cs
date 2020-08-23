using System;
namespace Banking.Customers.Domain.Interfaces
{
    public interface IServiceDataProtection
    {
        string CalculateHash(string input);
        bool CheckMatch(string hash, string input);

        string Encrypt<T>(T obj);
        string Encryp(string plaintext);
        bool TryDecrypt<T>(string encryptedText, out T obj);
        bool TryDecrypt(string encryptedText, out string decryptedText);
    }
}
