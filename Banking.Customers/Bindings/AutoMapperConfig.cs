using System;
using AutoMapper;
using AutoMapper.Configuration;
using Banking.Customers.Services.Mappers;

namespace Banking.Customers.Bindings
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<CustomerProfile>();
           // cfg.AddProfile(new CustomerProfile());


            return new MapperConfiguration(cfg);
        }
    }
}
