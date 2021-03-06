﻿using System.IO;
using System.Text;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Extensions;
using Banking.Customers.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Banking.Customers.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IServiceDataProtection _dataProtection;

        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IServiceDataProtection dataProtection)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
            _dataProtection = dataProtection;
        }

        public async Task InvokeAsync(HttpContext httpContext)  
        {
            httpContext.Request.Headers.TryGetValue("CLIENTNAME", out StringValues clientName);

            _logger.LogInformation($" PartnerName={clientName} Method={httpContext.Request.Method} ApiOperation={httpContext.Request.Path.Value}");

            var requestBody = httpContext.Request.SerializeBody();
            if (!string.IsNullOrEmpty(requestBody)) requestBody = requestBody.Replace('\n',' ').Replace('\r', ' ').Replace('\t',' ');
            //_logger.LogInformation($"Request:{requestBody}");

            requestBody = clientName;

            _logger.LogInformation($"Request:{_dataProtection.Encrypt(requestBody)}");

            //httpContext.Request.EnableBuffering(bufferThreshold: 1024 * 45, bufferLimit: 1024 * 100);
            ////using (var reader = new StreamReader(httpContext.Request.Body, encoding: Encoding.UTF8,
            ////        detectEncodingFromByteOrderMarks: false, bufferSize: bufferSize, leaveOpen: true))
            //using (var reader = new StreamReader(httpContext.Request.Body, encoding: Encoding.UTF8,
            //    detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            //{

            //    var requestBody = await  reader.ReadToEndAsync();   
            //    _logger.LogInformation($"Request:{requestBody}");
            //    httpContext.Request.Body.Position = 0;
            //}



            await _next.Invoke(httpContext);
        }

    }

}
