using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Banking.Customers.Tests
{
    public class UnitTestBase
    {
        protected readonly ITestOutputHelper Output;
        protected IServiceProvider serviceProvider;

        public UnitTestBase(ITestOutputHelper output)
        {
            Output = output;
        }



        public static IConfigurationRoot GetIConfigurationBase()
        {
            return new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
           .Build();
        }
    }
}
