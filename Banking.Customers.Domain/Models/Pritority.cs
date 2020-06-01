using System;
namespace Banking.Customers.Domain.Models
{
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PhoneType
    {
        Home,
        Work,
        Cell
    }

    public enum Priority
    {
        Low,
        Medium,
        High
    }
}
