using System;
using AutoMapper;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Models;
using Banking.Customers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Banking.Customers.Bindings
{
    public static class ServiceBindings
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSingleton<ICustomerService, CustomerService>();

            services.Configure<ApiInfo>(configuration.GetSection("ApiInfo"));


            //IServiceProvider serviceProvider = services.BuildServiceProvider();
            //var service = serviceProvider.GetService<IOptionsMonitor<ApiInfo>>();



            return services;
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<AutoMapper.IConfigurationProvider>(AutoMapperConfig.RegisterMappings());
        }
    }
}
