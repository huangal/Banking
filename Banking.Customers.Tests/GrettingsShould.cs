using System;
using System.Linq;
using System.Reflection;
using Banking.Customers.Data;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Banking.Customers.Tests
{
    public class GrettingsShould : UnitTestBase
    {

        public GrettingsShould(ITestOutputHelper output) : base(output)
        {
            serviceProvider = RegisterServices();
        }

        private void Init()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void PrivateHelloManShouldBeWellFormated()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";

            Type type = typeof(Greetings);
            var hello = Activator.CreateInstance(type, firstName, lastName);
            MethodInfo method = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(x => x.Name == "HelloMan" && x.IsPrivate)
            .First();

            //Act
            var helloMan = (string)method.Invoke(hello, new object[] { firstName, lastName });

            //Assert
            //helloMan.Should()
            //.StartWith("Hello")
            //.And
            //.EndWith("!")
            //.And
            //.Contain("John")
            //.And
            //.Contain("Doe");
        }

        [Fact]
        public void TestProtectedWriteHelloMethod()
        {
            var firstName = "John";
            var lastName = "Doe";

            var sut = new Greetings(firstName, lastName);


           var greeting = sut.WriteHello(firstName, lastName);



        }


        [Fact]
        public void DecryptDataUsingDataProtection()
        {
            string encrypted = "CfDJ8K3aWspf8pRNgo2HuquMzZl9kKzeSoZUILUFo90ADVzPmf5lJm7xhvvOIjtUZ_-D6ZYELZgj4iup_nPP-Stv6-OxJ4upTPrLL3_wwu7s2OPK298s2YoPoORhK70WYbbuRgP4PUWzO1ebRCgpX9J7j7zuji7xgE3tT0zJMfQX4mlU";

            var sut = serviceProvider.GetRequiredService<IServiceDataProtection>();

            string decrypted;

            var isDectypted = sut.TryDecrypt(encrypted, out decrypted);


            Assert.True(isDectypted);

            Output.WriteLine(decrypted);

            //

        }


        private ServiceProvider RegisterServices()
        {
            IServiceCollection services = new ServiceCollection();

            var iConfig = GetIConfigurationBase();

            services.AddSingleton<IConfiguration>(iConfig);
            services.AddSingleton<IServiceDataProtection, ServiceDataProtection>();
            services.AddSingleton<UniqueCode>();

            services.AddDataProtection();

            //services.AddDataProtection()
            //    .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
            //    {
            //        EncryptionAlgorithm = EncryptionAlgorithm.AES_256_GCM,
            //        ValidationAlgorithm = ValidationAlgorithm.HMACSHA512

            //    });

            return services.BuildServiceProvider();
        }



    }
}

