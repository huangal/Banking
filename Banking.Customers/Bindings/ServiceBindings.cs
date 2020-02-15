using System;
using AutoMapper;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Customers.Bindings
{
    public static class ServiceBindings
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {

            //var serviceAccessLevel = configuration.GetSection("ServiceAccessLevel")
            //                    .Get<ServiceAccessLevel>();

           // services.AddSingleton<IServiceAccessLevel>(cw => configuration.GetSection("ServiceAccessLevel").Get<ServiceAccessLevel>());
            //services.AddSingleton<IAuthorizationHandler, ServiceAccessHandler>();
           // services.AddScoped<ICustomerRepositoryService, CustomerRepositoryService>();

            services.AddScoped<ICustomerService, CustomerService>();
            return services;
        }


        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<AutoMapper.IConfigurationProvider>(AutoMapperConfig.RegisterMappings());
            services.AddSingleton(AutoMapperConfig.RegisterMappings());
        }
    }
}
