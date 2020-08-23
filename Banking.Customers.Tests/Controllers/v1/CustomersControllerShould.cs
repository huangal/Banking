using System;
using System.Threading.Tasks;
using Banking.Customers.Domain.Models;
using Banking.Customers.Tests.Brokers;
using FluentAssertions;
using Xunit;

namespace Banking.Customers.Tests.Controllers.v1
{


    [Collection(nameof(ApiTestCollection))]
    public class CustomersControllerShould
    {
        private readonly CustomerApiBroker _customerApiBroker;

        public CustomersControllerShould(CustomerApiBroker customerApiBroker) =>
            this._customerApiBroker = customerApiBroker;
        //public CustomersControllerShould(CustomerApiBroker customerApiBroker)
        //{
        //    this._customerApiBroker = customerApiBroker;
        //}



        [Fact]
        public async Task PostCustomerAsync()
        {
            //givem
            CustomerModel customer = CreateCustomer();
            CustomerModel inputCustomer = customer;
            CustomerModel expectedCustomer = customer;

            //when

            var currCustomer = await _customerApiBroker.GetCustomersAsync(1);

            CustomerModel actualCustomer = await _customerApiBroker.PostCustomerAsync(inputCustomer);


            //then
            expectedCustomer.Id = actualCustomer.Id;
            expectedCustomer.Email = actualCustomer.Email;

            actualCustomer.Should().BeEquivalentTo(expectedCustomer);



        }









        private CustomerModel CreateCustomer() =>
            new CustomerModel
            {
                Name = "Oso",
                Last = "Huangal",
                Age = 22,
                Email = "oso@huangal.com",
                Product = "VISA"
            };



    }
}
