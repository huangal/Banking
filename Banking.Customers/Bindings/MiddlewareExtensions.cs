using System;
using Banking.Customers.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Banking.Customers.Bindings
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogger(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<LoggerMiddleware>();
        }

        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestCultureMiddleware>();

            return builder;
        }

        public static IApplicationBuilder UseClientConfiguration(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ClientConfigurationMiddleware>();
        }

        public static IApplicationBuilder UseGlobalErrorHandler(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
    