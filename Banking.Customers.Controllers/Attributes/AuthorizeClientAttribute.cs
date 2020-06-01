using Banking.Customers.Controllers.Policies;
using Microsoft.AspNetCore.Authorization;

namespace Banking.Customers.Controllers.Attributes
{
    public class AuthorizeClientAttribute : AuthorizeAttribute
    {
        public AuthorizeClientAttribute()
        {
            Policy = PolicyType.PartnerAccess;
        }
    }
}
