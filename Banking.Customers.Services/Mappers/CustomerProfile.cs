using System;
using AutoMapper;
using Banking.Customers.Domain;
using Banking.Customers.Domain.Models;

namespace Banking.Customers.Services.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerModel>()
            .ReverseMap();
        }
    }
}
