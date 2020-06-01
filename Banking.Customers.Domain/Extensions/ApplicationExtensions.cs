using System;
using Microsoft.Extensions.Logging;

namespace Banking.Customers.Domain.Extensions
{
    public static class ApplicationExtensions
    {
        
        public static string ParseToPlain(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Replace("\"", "'")
                .Replace("\n", " ")
                .Replace("\r", " ");
        }


        public static void LogException(this ILogger logger, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            logger.LogError($"\"{value.ParseToPlain()}\"");
        }

        public static void LogException(this ILogger logger, object exception, string value = null)
        {
            if (string.IsNullOrWhiteSpace(value)) value = string.Empty;
            if (exception == null && !string.IsNullOrWhiteSpace(value))
            {
                logger.LogError($"\"{value.ParseToPlain()}\"");
                return;
            }

            if (exception is Exception)
                logger.LogError($"\"{value.ParseToPlain()}\"| Exception=\"{exception.ToString().ParseToPlain()}\"");
        }
    }
}
