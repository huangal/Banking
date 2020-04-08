using System;
using System.Text.Json;

namespace Banking.Customers.Domain.Models
{
    public class Status
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }


    public class ResponseStatus
    {
        public Guid? TransactionId { get; set; }
        public Status Status { get; set; } = new Status();

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
