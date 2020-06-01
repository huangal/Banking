using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Banking.Customers.Domain.Models;
using Banking.Customers.Controllers.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;


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

        public async Task Invoke(HttpContext httpContext)
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

            var transaction = GetTransactionId(httpContext);

            var response = new ResponseStatus { TransactionId = (Guid) transaction.TransactionId };

            //_logger.LogError(ex.ToString());

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

            if(ex is InvalidOperationException)
            {
                status.Code = (int)HttpStatusCode.BadRequest;
                status.Message = "Validation Failure";
               // status.Description = "Invalid Data Type. Please review your request and try again.";
            }

            // else if (ex is Exception) code = HttpStatusCode.BadRequest;
            response.Status = status;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = status.Code;
            await httpContext.Response.WriteAsync(response.ToString());

            response.Status.Message += $"  {httpContext.Request.Path.Value}";
          //  _logger.LogError($"Request:{response.ToString()}");

        }

        private Transaction GetTransactionId(HttpContext httpContext)
        {
            // Transaction transaction = new Transaction { TransactionId = Guid.NewGuid() };

            Transaction transaction = httpContext.Request.Deserialize<Transaction>();
            if (transaction == null) transaction = new Transaction { TransactionId = Guid.NewGuid() };
            if (!transaction.IsValidGuid()) transaction.TransactionId = Guid.NewGuid();

            return transaction;



            //httpContext.Request.EnableBuffering(bufferThreshold: 1024 * 45, bufferLimit: 1024 * 100);
            //using (var reader = new StreamReader(httpContext.Request.Body))
            //{
            //    string requestBody = reader.ReadToEndAsync().Result;
            //    if (!string.IsNullOrWhiteSpace(requestBody))
            //    {
            //        try
            //        {
            //            transaction = Newtonsoft.Json.JsonConvert.DeserializeObject<Transaction>(requestBody);
            //            if(transaction == null) transaction = new Transaction { TransactionId = Guid.NewGuid() };
            //            if (!transaction.IsValidGuid()) transaction.TransactionId = Guid.NewGuid();


            //        }
            //        catch (Exception)
            //        {
            //            transaction.TransactionId = Guid.Empty;
            //        }
            //    }
            //}

            //return transaction;
        }
    }
}
