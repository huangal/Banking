using System;
using Banking.Customers.Domain.Models;


namespace Banking.Customers.Domain.Interfaces
{
    public interface IClientConfiguration
    {
        string ClientName { get; set; }
        public Transaction ApiTransaction { get; set; }
        DateTime InvokedDateTime { get; set; }
    }

}
