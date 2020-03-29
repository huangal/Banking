using System;
using Banking.Customers.Domain.Interfaces;

namespace Banking.Customers.Models
{
    public class ClientConfiguration : IClientConfiguration
    {
        public string ClientName { get; set; }

        public DateTime InvokedDateTime { get; set; }
    }
}




