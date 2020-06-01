using System;
using System.Linq;
using System.Threading.Tasks;
using Banking.Customers.Domain.Constants;
using Banking.Customers.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Banking.Customers.Middleware
{
    public class ClientConfigurationMiddleware
    {
        private readonly RequestDelegate _next;

        public ClientConfigurationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IClientConfiguration clientConfiguration)
        {
            if (httpContext.Request.Headers.TryGetValue(ApplicationHelper.ClientHeader, out StringValues clientName))
            {
                clientConfiguration.ClientName = clientName.SingleOrDefault();
            }
            else
            {
               // throw new UnauthorizedAccessException("Unable to authenticate client");

            }

            clientConfiguration.InvokedDateTime = DateTime.UtcNow;
            await _next.Invoke(httpContext);
        }
    }
}
