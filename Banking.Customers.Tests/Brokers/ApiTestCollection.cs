using System;
using Xunit;

namespace Banking.Customers.Tests.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<CustomerApiBroker>
    {
        
    }
}
