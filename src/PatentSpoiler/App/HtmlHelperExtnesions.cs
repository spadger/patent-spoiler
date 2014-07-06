using System.Web;
using System.Web.Mvc;
using PatentSpoiler.App.Domain.Security;

namespace PatentSpoiler.App
{
    public static class HtmlHelperExtnesions
    {
        public static bool UserIsAuthenticated(this HtmlHelper @this)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static bool UserIs(this HtmlHelper @this, UserRole role)
        {
            return @this.UserIsAuthenticated() && HttpContext.Current.User.IsInRole(role.ToString());
        }
    }
}