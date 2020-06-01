using System;
using System.IO;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace Banking.Customers.Models
{
    /// <summary>
    /// An <see cref="ITextFormatter"/> that writes events in a compact JSON format, for consumption in environments 
    /// without message template support. Message templates are rendered into text and a hashed event id is included.
    /// </summary>
    public class CustomSerilogFormatter : ITextFormatter
    {
        private readonly JsonValueFormatter _valueFormatter;
        private readonly string _appName;
        private readonly string _appVersion;
        private readonly bool _renderMessageTemplate;

        private const int LogVersion = 2;

        /// <summary>
        /// Construct a <see cref="JsonValueFormatter"/>, optionally supplying a formatter for
        /// <see cref="LogEventPropertyValue"/>s on the event.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="appVersion">Version of the application.</param>
        /// <param name="renderMessageTemplate">Whether or not the message template should be rendered.</param>
        /// <param name="valueFormatter">A value formatter, or null.</param>
        public CustomSerilogFormatter(string appName, string appVersion, bool renderMessageTemplate = false, JsonValueFormatter valueFormatter = null)
        {
            _appName = appName;
            _appVersion = appVersion;
            _renderMessageTemplate = renderMessageTemplate;
            _valueFormatter = valueFormatter ?? new JsonValueFormatter(typeTagName: "$type");
        }

        /// <summary>
        /// Format the log event into the output. Subsequent events will be newline-delimited.
        /// </summary>
        /// <param name="logEvent">The event to format.</param>
        /// <param name="output">The output.</param>
        public void Format(LogEvent logEvent, TextWriter output)
        {
            FormatEvent(logEvent, output, _valueFormatter);
            output.WriteLine();
        }

        /// <summary>
        /// Format the log event into the output.
        /// </summary>
        /// <param name="logEvent">The event to format.</param>
        /// <param name="output">The output.</param>
        /// <param name="valueFormatter">A value formatter for <see cref="LogEventPropertyValue"/>s on the event.</param>
        public void FormatEvent(LogEvent logEvent, TextWriter output, JsonValueFormatter valueFormatter)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            if (output == null) throw new ArgumentNullException(nameof(output));
            if (valueFormatter == null) throw new ArgumentNullException(nameof(valueFormatter));

            output.Write("{\"app\":\"");
            output.Write(this._appName);
            output.Write("\",\"timestamp\":\"");
            output.Write(logEvent.Timestamp.UtcDateTime.ToString("O"));
            output.Write("\",\"message\":");
            var message = _renderMessageTemplate ? logEvent.MessageTemplate.Render(logEvent.Properties) : logEvent.MessageTemplate.ToString();
            JsonValueFormatter.WriteQuotedJsonString(message, output);

            output.Write(",\"level\":\"");
            output.Write(logEvent.Level);
            output.Write('\"');

            if (logEvent.Exception != null)
            {
                output.Write(",\"exception\":");
                JsonValueFormatter.WriteQuotedJsonString(logEvent.Exception.ToString(), output);
            }

            foreach (var property in logEvent.Properties)
            {
                var name = property.Key;
                if (name.Length > 0 && name[0] == '@')
                {
                    // Escape first '@' by doubling
                    name = '@' + name;
                }

                if (name == "RequestPath" || name == "CorrelationId") //Don't use Microsoft's Corr Id
                {
                    continue;
                }

                if (name == "SourceContext")
                {
                    name = "category";
                }

                if (name == "RequestId")
                {
                    name = "correlationId";
                }

                if (name.Length > 1)
                {
                    name = name.Substring(0, 1).ToLower() + name.Substring(1);
                }


                output.Write(',');
                JsonValueFormatter.WriteQuotedJsonString(name, output);
                output.Write(':');
                valueFormatter.Format(property.Value, output);
            }
            output.Write(",\"version\":\"");
            output.Write(_appVersion);
            output.Write("\",\"logVersion\":\"");
            output.Write(LogVersion);
            output.Write("\"}");
        }
    }


    public static class CustomSerilogConfigurator
    {
        private static readonly LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = LogEventLevel.Debug
        };

        public static LogEventLevel LoggingLevel
        {
            set => LoggingLevelSwitch.MinimumLevel = value;
        }

        public static void Setup(string appName, string appVersion, bool logToFile, string logFilePath = ".\\logs\\", bool renderMessageTemplate = false)
        {
            var formatter = new CustomSerilogFormatter(appName, appVersion, renderMessageTemplate);
            Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(LoggingLevelSwitch)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("IdentityServer4", LogEventLevel.Warning)
                .MinimumLevel.Override("HealthCheck.HealthCheckMiddleware", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Warning)
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.Logger(lc =>
                {
                    lc.Filter.ByExcluding(Matching.FromSource("performance"));
                    lc.WriteTo.Console(formatter);
                    if (logToFile)
                    {
                        lc.WriteTo.RollingFile(formatter, Path.Combine(logFilePath, "log-{Date}.log"));
                    }
                })
                .WriteTo.Logger(lc =>
                {
                    lc.Filter.ByIncludingOnly(Matching.FromSource("performance"));
                    lc.WriteTo.Console(formatter);
                    if (logToFile)
                    {
                        lc.WriteTo.RollingFile(formatter, Path.Combine(logFilePath, "log-perf-{Date}.log"));
                    }
                })
                .CreateLogger();
        }
    }
}
