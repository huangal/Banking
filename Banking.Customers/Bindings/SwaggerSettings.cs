using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Banking.Customers.OpenApi;

namespace Banking.Customers.Bindings
{
    public static class SwaggerSettings
    {
        public static void AddSwaggerSettings(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                options =>
                {
                    // add a custom operation filter which sets default values
                    options.OperationFilter<SwaggerDefaultValues>();
                    // integrate xml comments
                    XmlCommentsFiles.ForEach(file => options.IncludeXmlComments(file));
                });

        }

        public static void UseSwaggerSettings(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger( options => {
                options.SerializeAsV2 = false;
                options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{httpReq.PathBase}" }
                    };
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/banking/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    options.DocumentTitle = "Banking.Customers.API";
                }
                               
            });

            
            //Add swagger eDocumentation
            app.UseReDoc(options =>
            {
                options.SpecUrl = $"/banking/swagger/{provider.ApiVersionDescriptions.Last().GroupName}/swagger.json";
                options.DocumentTitle = "Banking.Customers";
                options.ConfigObject = new Swashbuckle.AspNetCore.ReDoc.ConfigObject()
                {
                };
            });
        }


        private static List<string> XmlCommentsFiles
        {
            get
            {             
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                return Directory.GetFiles(basePath, "*.xml", SearchOption.TopDirectoryOnly).ToList();
            }
        }
    }

    
}

