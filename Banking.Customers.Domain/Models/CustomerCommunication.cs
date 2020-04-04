using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Customers.Domain.Models
{
    public class CustomerCommunication
    {
        public string Value { get; set; }
        public Priority Priority { get; set; }
    }

    public enum PhoneType
    {
        notvalid,
        Home,
        Work,
        Cell
    }


    /// <summary>
    /// Phone Model
    /// </summary>
    public class Phone
    {
        [Required(ErrorMessage = "Required")]
        public string Number { get; set; }
        [Required(ErrorMessage = "Required")]
        public PhoneType Type { get; set; }
    }


}
