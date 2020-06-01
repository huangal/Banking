using Banking.Customers.Bindings;
using Banking.Customers.Data;
using Banking.Customers.Models;
using Banking.Customers.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;
using Banking.Customers.Filters;

namespace Banking.Customers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //public static void Setup(string appName, string appVersion, bool logToFile, string logFilePath = ".\\logs\\", bool renderMessageTemplate = false)
            CustomSerilogConfigurator.Setup("TestApp","1.4.3",true,"/Users/henryhuangal/Projects/AppLogs/BankingCustomer.txt");//     .Setup("doc-stack-app-api", false);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
            //    .ConfigureApiBehaviorOptions(options =>
            //    {
            //        options.SuppressModelStateInvalidFilter = true;
            //    })

            services.AddHttpContextAccessor();

            services.AddControllers(options => options.Filters.Add(new TrackPerformanceFilter()))
                .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                    
                 })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.Error = (sender, args) =>
                    {
                        
                       // throw new Exception("There was an error during deserialization.");
                        //throw new InvalidCastException("Unable to validate Data Type");
                    };
                });


               // .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddVersioning();

            services.RegisterServices(Configuration);

           services.AddAutoMapper();

           services.AddDbContext<CustomersContext>(context => { context.UseInMemoryDatabase("Customers"); });

            services.AddSwaggerSettings();
            services.AddAppAuthorization();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            

            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }

            app.UsePathBase("/Banking");

            app.UseGlobalErrorHandler();

            app.CreateSeedData();
            app.UseSwaggerSettings(provider);

            app.UseHttpsRedirection();
           
            app.UseRouting();

            
            app.UseAuthorization();

            //Register custom middleware
            // app.UseClientConfiguration();
             app.UseLogger();

            app.UseClientConfiguration();

            // app.UseJsonErrorHandler();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
