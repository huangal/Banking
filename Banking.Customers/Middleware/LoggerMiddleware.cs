using System.IO;
using System.Text;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

namespace Banking.Customers.Middleware
{
    public class LoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggerMiddleware>();
        }

        public async Task InvokeAsync(HttpContext httpContext)  
        {
            httpContext.Request.Headers.TryGetValue("CLIENTNAME", out StringValues clientName);

            _logger.LogInformation($" PartnerName={clientName} Method={httpContext.Request.Method} ApiOperation={httpContext.Request.Path.Value}");

            var requestBody = httpContext.Request.SerializeBody();
            _logger.LogInformation($"Request:{requestBody}");



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
