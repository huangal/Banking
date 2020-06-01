using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Banking.Customers.Controllers.Extensions
{
    public static class HttpContextExtensions
    {
        public static T Deserialize<T>(this HttpRequest request) where T : class
        {
            try
            {
                var requestBody = request.SerializeBody();
                request.Body.Position = 0;
                if (string.IsNullOrWhiteSpace(requestBody)) return default;

                var response = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(requestBody);

                return response ?? (default);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public static string SerializeBody(this HttpRequest request)
        {
            request.EnableBuffering(bufferThreshold: 1024 * 45, bufferLimit: 1024 * 100);
            using (var reader = new StreamReader(request.Body, encoding: Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                try
                {
                    var requestBody = reader.ReadToEndAsync().Result;
                    request.Body.Position = 0;

                    return requestBody;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }
    }


}
