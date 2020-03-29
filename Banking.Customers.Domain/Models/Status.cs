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
}
