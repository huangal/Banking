using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Banking.Customers.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Banking.Customers.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IWebHostEnvironment env)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
            _env = env;
        }

        public async Task Invoke(HttpContext httpContext /* other dependencies */)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            _logger.LogError(ex.ToString());

            var status = new Status
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "Internal Server Error",
                Description = "An internal server error has occurred.  Please, try later."
            };

            if (_env.IsDevelopment())
            {
                status.Description = ex.ToString();
            }

            //if (ex is NotFoundException) code = HttpStatusCode.NotFound;
            if (ex is UnauthorizedAccessException)
            {
                status.Code = (int)HttpStatusCode.Unauthorized;
                status.Message = "Unauthorized";
                status.Description = ex.Message;
            }
            // else if (ex is Exception) code = HttpStatusCode.BadRequest;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = status.Code;
            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(status));
        }
    }
}
