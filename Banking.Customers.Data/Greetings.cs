using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Banking.Customers.Tests")]

namespace Banking.Customers.Data
{
    public class Greetings
    {
        private string _firstName { get; set; }
        private string _lastName { get; set; }

        public Greetings(string firstName, string lastName)
        {
            _firstName = firstName;
            _lastName = lastName;
        }

        public string HelloMan()
        {
            if (string.IsNullOrEmpty(_firstName))
                throw new MissingFirstNameException();

            return this.HelloMan(_firstName, _lastName);
        }

        private string HelloMan(string firstName, string lastName)
        {
            return $"Hello {firstName} {lastName} !";
        }

        internal string WriteHello(string firstName, string lastName)
        {
            return $"Hello {firstName} {lastName} !";
        }
    }




    public class MissingFirstNameException : Exception
    {
        public MissingFirstNameException() : base("FirstName is missing")
        {
        }
    }
}
