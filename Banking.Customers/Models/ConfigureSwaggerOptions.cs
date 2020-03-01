using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Banking.Customers.Models
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration) {
             this._provider = provider;
            this._configuration = configuration;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        private  OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {

            var apiInfo = _configuration.GetSection("ApiInfo").Get<ApiInfo>();


            var openApiInfo = new OpenApiInfo
            {
                Title = apiInfo.Title,
                Version = description.ApiVersion.ToString(),
                Description = apiInfo.Description,
                TermsOfService = new Uri(apiInfo.TermsOfService),
                Contact = new OpenApiContact
                {
                    Name = apiInfo.Contact.Name,
                    Email = apiInfo.Contact.Email,
                    Url = new Uri(apiInfo.Contact.Url),
                },
                License = new OpenApiLicense
                {
                    Name = apiInfo.License.Name,
                    Url = new Uri(apiInfo.License.Url),
                }
            };

                                          
            if (description.IsDeprecated)
            {
                openApiInfo.Description += " This API version has been deprecated.";
            }

            return openApiInfo;
        }

          
    }

    public class ApiInfo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TermsOfService { get; set; }
        public Contact Contact { get; set; }
        public License License { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
    }

    public class License
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}




