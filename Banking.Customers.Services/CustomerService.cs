using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Banking.Customers.Data;
using Banking.Customers.Data.Extensions;
using Banking.Customers.Domain;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
using Microsoft.EntityFrameworkCore;



namespace Banking.Customers.Services
{
    public class CustomerService: ICustomerService
    {
        private readonly CustomersContext _context;
        private readonly IMapper _mapper;
        private readonly IConfigurationProvider _mapperConfiguration;

        public CustomerService(CustomersContext context, IMapper mapper, IConfigurationProvider mapperConfig)
        {
            _context = context;
            _mapper = mapper;
            _mapperConfiguration = mapperConfig;
            _context.CreateSeedData();
        }

        public async Task<CustomerModel> GetCustomersAsync(int id)
        {
            return await _context.Customers.Where(x => x.Id == id)
                .ProjectTo<CustomerModel>(_mapperConfiguration)
                .FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersAsync()
        {
            return await _context.Customers.ProjectTo<CustomerModel>(_mapperConfiguration).ToListAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomerListAsync(int count)
        {
            return await _context.Customers.Take(count).ProjectTo<CustomerModel>(_mapperConfiguration).ToListAsync();
        }


        public async Task<IEnumerable<CustomerModel>> GetByPartialName(string titleFragment)
        {
            return await _context.Customers.Where(x => x.Name.IndexOf(titleFragment, 0, StringComparison.OrdinalIgnoreCase) >= 0)
                .ProjectTo<CustomerModel>(_mapperConfiguration)
                .ToListAsync();
                                
        }


        public int GetCustomersCount()
        {
            return _context.Customers.Count();

        }

        public async Task<CustomerModel> CreateCustomerAsync(CustomerModel customerModel)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Name.Equals(customerModel.Name)
           && x.Last.Equals(customerModel.Last));

            if (customer != null) return _mapper.Map<CustomerModel>(customer);

            Customer newCustomer = _mapper.Map<Customer>(customerModel);
            int id = _context.Customers.Max(x => x.Id);
            newCustomer.Id = id + 1;

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerModel>(newCustomer);

        }

        public async Task<CustomerModel> UpdateCustomerAsync(CustomerModel customerModel)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerModel.Id);

            if (customer == null) return null;

            _context.Entry(customer).State = EntityState.Detached;
            customer = _mapper.Map<Customer>(customerModel);

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
            return _mapper.Map<CustomerModel>(customer);
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var item = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);
            if (item == null) return false;

            _context.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        

    }
}
