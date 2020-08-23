using System;
using System.Threading.Tasks;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Tests.Brokers
{
    public partial class CustomerApiBroker
    {
        private const string CustomerRelativeUrl = "/api/v1/Customers";


        public async ValueTask<CustomerModel> PostCustomerAsync(CustomerModel customer) =>
            await this.apiFactoryClient.PostContentAsync(CustomerRelativeUrl, customer);


        public async ValueTask<CustomerModel> GetCustomersAsync(int id) =>
            await this.apiFactoryClient.GetContentAsync<CustomerModel>($"{CustomerRelativeUrl}/{id}");



    }
}
