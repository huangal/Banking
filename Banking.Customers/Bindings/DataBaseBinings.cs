using Banking.Customers.Data;
using Banking.Customers.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Customers.Bindings
{
    public static class DataBaseBinings
    {
        public static void CreateSeedData(this IApplicationBuilder app)
        {
            var dataContext = app.ApplicationServices.GetService<CustomersContext>();
            dataContext.CreateSeedData(2500);
        }

    }
}
