using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Extensions;
using Banking.Customers.Domain.Constants;
using Banking.Customers.Domain.Interfaces;
using Banking.Customers.Domain.Models;
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
            if (httpContext.Request.Headers.TryGetValue(HeatherType.ClientHeader, out StringValues clientName))
            {
                clientConfiguration.ClientName = clientName.SingleOrDefault();
            }
            else
            {
                // throw new UnauthorizedAccessException("Unable to authenticate client");

            }




            var requestBody = httpContext.Request.SerializeBody();
            if (!string.IsNullOrEmpty(requestBody))
            {
                requestBody = requestBody.Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');

                clientConfiguration.ApiTransaction = JsonSerializer.Deserialize<Transaction>(requestBody);


            }
            //_logger.LogInformation($"Request:{requestBody}");




            clientConfiguration.InvokedDateTime = DateTime.UtcNow;
            await _next.Invoke(httpContext);
        }
    }
}
