using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Banking.Customers.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Banking.Customers.OpenApi.Models;

namespace Banking.Customers.Bindings
{
    public static class SwaggerSettings
    {
        public static IServiceCollection AddSwaggerSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigurationSwaggerOptions>();
            services.Configure<ApiInfo>(configuration.GetSection("ApiInfo"));

            services.AddSwaggerGen();

            #region move to configuration swagger options
            //services.AddSwaggerGen(
            //    options =>
            //    {
            //        // add a custom operation filter which sets default values
            //       // options.OperationFilter<SwaggerDefaultValues>();
            //        //options.OperationFilter<AddRequiredHeaderParameter>();


            //        //// integrate xml comments
            //        //XmlCommentsFiles.ForEach(file => options.IncludeXmlComments(file, includeControllerXmlComments: true));


            //        //options.AddSecurityDefinition("PartnerName", new OpenApiSecurityScheme()
            //        //{

            //        //    Type = SecuritySchemeType.ApiKey,
            //        //    Scheme = "PartnerName",
            //        //    Description = "Input your key  to access this API"


            //        //});

            //        //options.AddSecurityRequirement(new OpenApiSecurityRequirement
            //        //{
            //        //    {
            //        //        new OpenApiSecurityScheme
            //        //        {
            //        //            Reference = new OpenApiReference {
            //        //                Type =  ReferenceType.SecurityScheme,
            //        //                Id = "PartnerName" }
            //        //        }, new List<string>()
            //        //        }
            //        //});

            //    });
            #endregion

            return services;

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

                options.DefaultModelExpandDepth(2);
                options.DefaultModelRendering(Swashbuckle.AspNetCore.SwaggerUI.ModelRendering.Model);
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);

                //options.EnableDeepLinking();
                //options.DisplayOperationId();
                               
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


        //private static List<string> XmlCommentsFiles
        //{
        //    get
        //    {             
        //        var basePath = PlatformServices.Default.Application.ApplicationBasePath;
        //        return Directory.GetFiles(basePath, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        //    }
        //}
    }


    //public class AddRequiredHeaderParameter : IOperationFilter
    //{
    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //    {

            

            

    //        //   var hasAuthorize =
    //        //context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any()
    //        //|| context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

    //        //   if (hasAuthorize)
    //        //   {
    //        //       operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
    //        //       operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

    //        //       operation.Security = new List<OpenApiSecurityRequirement>
    //        //   {
    //        //       new OpenApiSecurityRequirement
    //        //       {
    //        //           [
    //        //               new OpenApiSecurityScheme {Reference = new OpenApiReference
    //        //               {
    //        //                   Type = ReferenceType.SecurityScheme,
    //        //                   Id = "oauth2"}
    //        //               }
    //        //           ] = new[] {"api1"}
    //        //       }
    //        //   };

    //        //   }




    //        if (operation.Parameters == null)
    //            operation.Parameters = new List<OpenApiParameter>();


    //        IDictionary<string, OpenApiExample> examples = new Dictionary<string, OpenApiExample>();
    //        examples.Add("par1", new OpenApiExample {
    //            Description = "CIS Number:   CISNumber|12345678",
    //            Summary = "CISNumber"
    //        });

    //        examples.Add("par2", new OpenApiExample
    //        {
    //            Description = "ELB Number:   ELBNumber|12345678",
    //            Summary = "ELBNumber"
    //        });
    //        examples.Add("par3", new OpenApiExample
    //        {
    //            Description = "Routing and ELb:   RoutingAndElbNumber|101000695:123456781231231",
    //            Summary = "RoutingAndElbNumber"
    //        });


    //        operation.Parameters.Add(new OpenApiParameter
    //        {
    //            Name = "x-partner-name",
    //            Description = "Client resgistered name",
    //            In = ParameterLocation.Header,
    //            Required = true,
    //            Schema = new OpenApiSchema() { Type = "string"}
    //        });
    //        operation.Parameters.Add(new OpenApiParameter
    //        {
    //            Name = "x-customer-id",
    //            Description = "Customer Id",
    //            In = ParameterLocation.Header,
    //            Required = true,
    //            Schema = new OpenApiSchema() { Type = "string" },
    //            Examples = examples
    //        });



    //        IDictionary<string, OpenApiExample> requestIdSample = new Dictionary<string, OpenApiExample>();
    //        examples.Add("part1", new OpenApiExample
    //        {
    //            Description = "2c4d133b-52e6-4e8a-a88b-1e69c069aeae",
    //            Summary = "UUID"
    //        });
    //        operation.Parameters.Add(new OpenApiParameter
    //        {
    //            Name = "RequestId",
    //            Description = "Operation id",
    //            In = ParameterLocation.Header,
    //            Required = true,
    //            Schema = new OpenApiSchema() { Type = "string" },
    //            Examples = requestIdSample
    //        }); 

    //    }
  
    //}

    

}

