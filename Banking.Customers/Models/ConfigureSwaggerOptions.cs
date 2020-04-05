using System;
using Banking.Enterprise.Configuration;
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
        private readonly ApiInfo _apiInfo;
        private readonly Person _person;


        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="configuration"></param>
        /// <param name="optionsAccessor"></param>
        /// <param name="configOptions"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider,
                    IConfiguration configuration,
                    IOptionsMonitor<ApiInfo> optionsAccessor,
                    IConfigOptions<Person> configOptions)
        {
             _provider = provider;
            _configuration = configuration;
            _apiInfo = optionsAccessor.CurrentValue;
            _person = configOptions.Value;
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
            var openApiInfo = new OpenApiInfo
            {
                Title = _apiInfo.Title,
                Version = description.ApiVersion.ToString(),
                Description = _apiInfo.Description,
                TermsOfService = new Uri(_apiInfo.TermsOfService),
                Contact = new OpenApiContact
                {
                    Name = _apiInfo.Contact.Name,
                    Email = _apiInfo.Contact.Email,
                    Url = new Uri(_apiInfo.Contact.Url),
                },
                License = new OpenApiLicense
                {
                    Name = _apiInfo.License.Name,
                    Url = new Uri(_apiInfo.License.Url),
                }
            };

                                          
            if (description.IsDeprecated)
            {
                openApiInfo.Description += " This API version has been deprecated.";
            }

            return openApiInfo;
        }

          
    }
}




