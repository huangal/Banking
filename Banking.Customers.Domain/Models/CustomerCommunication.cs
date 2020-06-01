using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Customers.Domain.Models
{

    public class CustomerDepartment: Transaction
    {
        public CustomerCommunication customerCommunication { get; set; }
    }


    public class CustomerCommunication
    {
        [Required(ErrorMessage = "Required")]
        public string Value { get; set; }

        [Required(ErrorMessage = "Required")]
        public Priority? Priority { get; set; }

        [Required(ErrorMessage = "Required")]
        public Department? Department { get; set; }
    }


    public class Transaction
    {
        public Guid? TransactionId { get; set; }
        public bool IsValidGuid()
        {
            return (Guid.TryParse(TransactionId.ToString(), out var guid) && guid != Guid.Empty);
        }
    }


    /// <summary>
    /// Phone Model
    /// </summary>
    public class Phone //: IValidatableObject
    {
        [Required(ErrorMessage = "Required")]
        public string Number { get; set; }

        
        [Required(ErrorMessage = "Required type")]
        public PhoneType? PhoneType { get; set; }

    }
}
