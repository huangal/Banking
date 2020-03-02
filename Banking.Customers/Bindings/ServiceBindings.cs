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

            services.AddSingleton<IConfigOptions<ApiInfo>, AppConfigurations<ApiInfo>>();


            services.Configure<ApiInfo>(configuration.GetSection("ApiInfo"));
            services.Configure<ApiInfo>();



            return services;
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            services.AddSingleton<AutoMapper.IConfigurationProvider>(AutoMapperConfig.RegisterMappings());
        }


        private static ApiInfo LoadApiInfo(IConfiguration configuration)
        {

            var apiInfo = configuration.GetSection("ApiInfo").Get<ApiInfo>();
            return apiInfo;
        }


        private static void DBConfigure<T>(this IServiceCollection services) where T: class
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            services.AddSingleton<T>(configuration.GetSection("ApiInfo").Get<T>());

           
        }

        private static void Configure<T>(this IServiceCollection services) where T : class
        {
            services.AddSingleton<IConfigOptions<ApiInfo>, AppConfigurations<ApiInfo>>(); 
        }

    }

    public class AppConfigurations<T> : IConfigOptions<T> 
    {
        private IConfiguration Configuration;

        public AppConfigurations(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public T CurrentValue 
        {
            get { return Configuration.GetSection("ApiInfo").Get<T>(); }
        }
    }

    public interface IConfigOptions<T>
    {
         T CurrentValue { get; }
    }

}
