using System;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Models
{
    public class ClientConfiguration : IClientConfiguration
    {
        public string ClientName { get; set; }
        public DateTime InvokedDateTime { get; set; }
        public Transaction ApiTransaction { get; set; }
    }
}




