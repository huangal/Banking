using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Banking.Customers.Middleware
{
    public class JsonExceptionMiddleware
    {

        private readonly RequestDelegate _next;

        public JsonExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
            if (ex == null) return;

            var error = new
            {
                message = ex.Message
            };

            context.Response.ContentType = "application/json";

            using (var writer = new StreamWriter(context.Response.Body))
            {
                new JsonSerializer().Serialize(writer, error);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

        //public async Task InvokeAsync(HttpContext httpContext, IClientConfiguration clientConfiguration)
        //{
        //    if (httpContext.Request.Headers.TryGetValue("CLIENTNAME", out StringValues clientName))
        //    {
        //        clientConfiguration.ClientName = clientName.SingleOrDefault();
        //    }
        //    else
        //    {
        //        throw new UnauthorizedAccessException("Unable to authenticate client");

        //    }

        //    clientConfiguration.InvokedDateTime = DateTime.UtcNow;
        //    await _next.Invoke(httpContext);
        //}
    }
}
