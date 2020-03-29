using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Bindings;
using Banking.Customers.Controllers.Attributes;
using Banking.Customers.Data;
using Banking.Customers.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
using System.Text.Json;
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
            // services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true;
                })
                .AddXmlSerializerFormatters();
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
                //app.UseDeveloperExceptionPage();
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context => await HandleGlobalErrorAsync(context));
                });
            }

            app.CreateSeedData();
            app.UseSwaggerSettings(provider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseLogger();
            app.UseClientConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private async Task HandleGlobalErrorAsync(HttpContext context)
        {

            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

            var status = new Status { Code = 500, Message = "Internal Server Error", Description = exceptionHandlerPathFeature.Error.ToString() };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            if (exceptionHandlerPathFeature?.Error is UnauthorizedAccessException)
            {
                context.Response.StatusCode = 401;
                status.Code = 401;
                status.Message = "Unauthorized";
                status.Description = exceptionHandlerPathFeature.Error.Message;
            }

            await context.Response.WriteAsync(JsonSerializer.Serialize(status));
        }
    }
}
