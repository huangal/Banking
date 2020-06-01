using System;
using System.Threading.Tasks;
using Banking.Customers.Controllers.Extensions;
using Banking.Customers.Domain.Constants;
using Banking.Customers.Domain.Models;
using BBanking.Customers.Controllers.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Banking.Customers.Controllers.Policies
{
    public class PartnerAccessHandler : AuthorizationHandler<PartnerAccessRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PartnerAccessHandler(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PartnerAccessRequirement requirement)
        {

            if (requirement.HasPartnerAccess(_httpContextAccessor))
            {

                var contact = _httpContextAccessor.HttpContext.Request.Deserialize<CustomerCommunication>();


                context.Succeed(requirement);
            }
            else
            {
                throw new UnauthorizedAccessException("Unable to authenticate client");
            }

            return Task.CompletedTask;
        }
    }

}
