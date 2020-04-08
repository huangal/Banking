using Banking.Customers.Bindings;
using Banking.Customers.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

namespace Banking.Customers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
            services.AddControllers()
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


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {

            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }

            app.UseGlobalErrorHandler();

            app.CreateSeedData();
            app.UseSwaggerSettings(provider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Register custom middleware
           // app.UseClientConfiguration();
            app.UseLogger();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
