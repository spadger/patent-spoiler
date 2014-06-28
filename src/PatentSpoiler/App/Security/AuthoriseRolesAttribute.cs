using System.Web.Mvc;
using PatentSpoiler.App.Domain.Security;

namespace PatentSpoiler.App.Security
{
    public class AuthoriseRolesAttribute : AuthorizeAttribute
    {
        public AuthoriseRolesAttribute(params UserRole[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}