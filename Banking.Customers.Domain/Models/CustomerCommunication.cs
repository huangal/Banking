using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Banking.Customers.Domain.Attributes;

namespace Banking.Customers.Domain.Models
{
    public class CustomerCommunication
    {
        public string Value { get; set; }
        public Priority Priority { get; set; }
    }


    public enum Department
    {
        SALES,
        FINANCE,
        ENGINEERING,
        MARKETING
    }

    // [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PhoneType
    {
        Home,
        Work,
        Cell
    }


    /// <summary>
    /// Phone Model
    /// </summary>
    public class Phone : IValidatableObject
    {
        [Required(ErrorMessage = "Required")]
        public string Number { get; set; }

        
        [Required(ErrorMessage = "Required type")]
        public PhoneType? PhoneType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (!IsValidResourceGroup(PhoneType.ToString()))
            {
                results.Add(new ValidationResult($"Invalid resource group"));
            }
            return results;
        }


        bool IsValidResourceGroup(string group)
        {
            foreach (var c in Enum.GetValues(typeof(PhoneType)))
            {
                if (string.Equals(group.Trim(), $"{c}", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }



    }

   

}
