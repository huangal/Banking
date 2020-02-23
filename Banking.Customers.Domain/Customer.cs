using System;
namespace Banking.Customers.Domain
{
    public class Customer
    {
        private string email;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Last { get; set; }
        public int Age { get; set; }
        public string Email { get => email; set => email = GetEmail(value); }
        public string Product { get; set; }


        private string GetEmail(string value)
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Last) ? $"{Name}.{Last}{value.Substring(value.IndexOf("@"))}" : value;
        }
    }

}
