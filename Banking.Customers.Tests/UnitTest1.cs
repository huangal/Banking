using System;
using System.Linq;
using System.Reflection;
using Banking.Customers.Data;
using Xunit;

namespace Banking.Customers.Tests
{
    public class GrettingsShould
    {
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


    }
}

