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
        
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated && !user.IsInRole(UserRole.EmailConfirmed.ToString()))
            {
                filterContext.Result = new RedirectResult("/account/verify", false);
            }
            else
            {
                base.OnAuthorization(filterContext);
            }
        }
    }
}