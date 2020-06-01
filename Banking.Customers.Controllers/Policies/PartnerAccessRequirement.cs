using Banking.Customers.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BBanking.Customers.Controllers.Policies
{
    public class PartnerAccessRequirement : IAuthorizationRequirement
    {
        internal bool HasPartnerAccess(IHttpContextAccessor httpContextAccessor)
        {
            string clientName = httpContextAccessor.HttpContext.Request.Headers[ApplicationHelper.ClientHeader];
            return !string.IsNullOrEmpty(clientName);
        }
    }
}
