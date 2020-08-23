using System;
using System.Net;

namespace Banking.Customers.Tests.Extenstions
{
    public static class HttpStatusExtensions
    {

        public static HttpStatusCode ToStatusCode(this string code)
        {
            if(int.TryParse(code, out int value))
            {
                if( Enum.IsDefined(typeof(HttpStatusCode), value))
                {

                    return (HttpStatusCode)value;
                }
            }

            return HttpStatusCode.BadRequest;
        }

        public static T ToEnum<T>(this string enumString)
        {
            return (T)Enum.Parse(typeof(T), enumString);
        }

    }
}
