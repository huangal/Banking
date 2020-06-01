
using Banking.Customers.Controllers.Policies;
using Banking.Customers.Domain.Constants;
using BBanking.Customers.Controllers.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Banking.Customers.Bindings
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyType.PartnerAccess,
                    policy =>
                    {
                        policy.Requirements.Add(new PartnerAccessRequirement());

                    });
            });


            services.AddScoped<IAuthorizationHandler, PartnerAccessHandler>();
            return services;
        }
    }
}
