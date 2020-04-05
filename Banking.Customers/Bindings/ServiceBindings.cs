using System;
using AutoMapper;
using Banking.Customers.Controllers.Managers;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Middleware;
using Banking.Customers.Models;
using Banking.Customers.Services;
using Banking.Enterprise.Configuration.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Banking.Customers.Bindings
{
    public static class ServiceBindings
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSingleton<ICustomerService, CustomerService>();



            //Log.Logger = new LoggerConfiguration()
            //.Enrich.FromLogContext()
            //.WriteTo.Console()
            //.CreateLogger();

            services.RegisterLogger(configuration);


            services.AddScoped<IClientConfiguration, ClientConfiguration>();

            // services.AddSingleton<IConfigOptions<ApiInfo>, AppConfigurations<ApiInfo>>();

            //services.AddSingleton<IConfigOptions<ApiInfo>, ConfigOptions<ApiInfo>>();

            services.AddSingleton<IGreeting, Saludos>();

            services.AddSingleton<IGreeting<EnglishGreetings>, EnglishGreetings>();


            services.Configure<ApiInfo>(configuration.GetSection("ApiInfo"));
            services.Configure<Person>();


            services.AddHttpClient<IWeatherForecastService, WeatherForecastService>()
                .ConfigurePrimaryHttpMessageHandler(handler => new MockWeatherForecastHandler());
            services.AddSingleton<IWeatherManager, WeatherManager>();


            //services.AddHttpClient<IConfigurationService, ConfigurationService>()
            //   .ConfigurePrimaryHttpMessageHandler(handler => new Return200ResponseHandler());

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


        public static IServiceCollection RegisterLogger(this IServiceCollection services, IConfiguration configuration)
        {
            //var logger = new LoggerConfiguration();
            //#if DEBUG
            //            string logfile = "/Users/henryhuangal/Projects/AppLogs/Banking-Customer-.log";
            //            logger.WriteTo.File(logfile, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
            //                  .WriteTo.Console();
            //#else
            //logger.ReadFrom.Configuration(configuration);
            //#endif

            //Log.Logger = logger.CreateLogger();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            return services;
        }


        //private static void Configure<T>(this IServiceCollection services) where T : class
        //{
        //    services.AddSingleton<IConfigOptions<ApiInfo>, AppConfigurations<ApiInfo>>(); 
        //}

    }

}
