using System;
using System.ComponentModel.DataAnnotations;

namespace Banking.Customers.Domain.Models
{
    public class CustomerModel
    {
        [Required] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Last { get; set; }


        [Range(18, 125, ErrorMessage = "Customer must be at least 18 years old.")]
        [Required]
        //[RegularExpression("/(((0|1)[0-9]|2[0-9]|3[0-1])\\/(0[1-9]|1[0-2])\\/((19|20)\\d\\d))$/", ErrorMessage = "Customer must be at least 18 years old.")]
        public int Age { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required] public string Product { get; set; }
    }
}
