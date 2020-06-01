using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

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





    public class ReleaseNumberEnricher : ILogEventEnricher
    {
        LogEventProperty _cachedProperty;

        public const string PropertyName = "ReleaseNumber";

        /// <summary>
        /// Enrich the log event.
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param>
        /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            // Don't care about thread-safety, in the worst case the field gets overwritten and one property will be GCed
            if (_cachedProperty == null)
                _cachedProperty = CreateProperty(propertyFactory);

            return _cachedProperty;
        }

        // Qualify as uncommon-path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            var value = Environment.GetEnvironmentVariable("RELEASE_NUMBER") ?? "local";
            return propertyFactory.CreateProperty(PropertyName, value);
        }
    }


    public static class LoggingExtensions
    {
        public static LoggerConfiguration WithReleaseNumber(this LoggerEnrichmentConfiguration enrich)
        {
            if (enrich == null)throw new ArgumentNullException(nameof(enrich));

            return enrich.With<ReleaseNumberEnricher>();
        }
    }

    //public class SplunkFormatter : IFormatProvider
    //{
    //    //public void Format(LogEvent logEvent, TextWriter output)
    //    //{
    //    //    var data = output;
    //    //}

    //    readonly IFormatProvider basedOn;
    //    readonly string shortDatePattern;

    //    public SplunkFormatter()
    //    {
    //        this.shortDatePattern = "dd-MMM-yyyy";
    //        this.basedOn = new CultureInfo("en-AU");
    //    }

    //    public object GetFormat(Type formatType)
    //    {
    //        var data = formatType.FullName;
    //        if (formatType == typeof(DateTimeFormatInfo))
    //        {
    //            var basedOnFormatInfo = (DateTimeFormatInfo)basedOn.GetFormat(formatType);
    //            var dateFormatInfo = (DateTimeFormatInfo)basedOnFormatInfo.Clone();
    //            dateFormatInfo.ShortDatePattern = this.shortDatePattern;
    //            return dateFormatInfo;
    //        }
    //        return this.basedOn.GetFormat(formatType);
    //    }
    //}

}
