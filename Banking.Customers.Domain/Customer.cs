using System;
namespace Banking.Customers.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Last { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Product { get; set; }
    }
}
