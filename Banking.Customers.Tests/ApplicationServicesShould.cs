using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Xunit;
using Xunit.Abstractions;
using Banking.Customers.Tests.Extenstions;

namespace Banking.Customers.Tests
{
    public class ApplicationServicesShould 
    {
        private readonly ITestOutputHelper Output;

        public ApplicationServicesShould(ITestOutputHelper output)
        {
            Output = output;
        }

        [Theory]
        [InlineData(200)]
        [InlineData(800)]
        [InlineData(12)]
        [InlineData(404)]
        public void Test1(int value)
        {

            Output.WriteLine($"Value: {value}");
            bool result = Enum.IsDefined(typeof(HttpStatusCode), value);

            Assert.True(result);


        }


        [Theory]
        [InlineData("200")]
        [InlineData("404")]
        [InlineData("1500")]
        [InlineData("abc")]
        public void GetHttpStatusCode(string value)
        {

            var code = value.ToStatusCode();

            Assert.IsType<HttpStatusCode>(code);

            var statusCode = value.ToEnum<HttpStatusCode>();

            Assert.IsType<HttpStatusCode>(statusCode);

        }


        [Fact]
        public void Test12()
        {
            int expected = StatusCodes.Status404NotFound;

            int code = 400;


            HttpStatusCode statusCode = HttpStatusCode.NotFound;

            int value = 200;
            bool result = Enum.IsDefined(typeof(HttpStatusCode), value);

            Assert.True(result);

        }



    }
}
