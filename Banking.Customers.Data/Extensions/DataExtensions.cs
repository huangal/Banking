using System.Collections.Generic;
using System.Linq;
using Banking.Customers.Domain;
using GenFu;

namespace Banking.Customers.Data.Extensions
{
    public static class DataExtensions
    {
        public static void CreateSeedData(this CustomersContext context)
        {

            if (context.Customers.Any()) return;

            var customers = LoadCustomers(1000);//LoadtCustomers(100);

            context.AddRange(customers);
            context.SaveChanges();

        }

        private static List<string> GetProducts()
        {
            return new List<string>()
            {
                "Visa Card",
                "Goal Saving Card",
                "Healthcare Card",
                "Rewards Card",
                "Saving Cards"
            };

        }
        private static IEnumerable<Customer> LoadCustomers(int count)
        {
            int id = 1;
            var products = GetProducts();

            GenFu.GenFu.Configure<Customer>()
                .Fill(x => x.Id, () => { return id++; })
                .Fill(x => x.Name).AsFirstName()
                .Fill(x => x.Last).AsLastName()
                .Fill(x => x.Age).WithinRange(18, 70)
                .Fill(x => x.Email).AsEmailAddress()
                .Fill(x => x.Product).WithRandom(products);

            List<Customer> customers = GenFu.GenFu.ListOf<Customer>(count);
            return customers;
        }
    }
}
