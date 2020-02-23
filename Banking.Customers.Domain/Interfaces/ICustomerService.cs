using System.Collections.Generic;
using System.Threading.Tasks;
using Banking.Customers.Domain.Models;


namespace Banking.Customers.Domain.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerModel> GetCustomersAsync(int id);
        Task<IEnumerable<CustomerModel>> GetCustomersAsync();
        Task<IEnumerable<CustomerModel>> GetCustomerListAsync(int count);
        int GetCustomersCount();
        Task<CustomerModel> CreateCustomerAsync(CustomerModel customerModel);
        Task<CustomerModel> UpdateCustomerAsync(CustomerModel customerModel);
        Task<bool> DeleteCustomerAsync(int id);
      
    }
}
