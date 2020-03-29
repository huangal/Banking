using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Banking.Customers.Middleware
{
    //public class AppConfigurations<T> : IConfigOptions<T> 
    //{
    //    private IConfiguration Configuration;

    //    public AppConfigurations(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }

    //    public T CurrentValue 
    //    {
    //        get { return Configuration.GetSection("ApiInfo").Get<T>(); }
    //    }
    //}

    //public interface IConfigOptions<T>
    //{
    //     T CurrentValue { get; }
    //}

    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }

}
