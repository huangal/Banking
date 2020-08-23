using System;

namespace Banking.Customers.Domain.Models
{
    public class Transaction
    {
        public Guid? TransactionId { get; set; }
        public bool IsValidGuid()
        {
            return (Guid.TryParse(TransactionId.ToString(), out var guid) && guid != Guid.Empty);
        }
    }
}
