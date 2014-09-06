using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PatentSpoiler.App.Domain.Security;

namespace PatentSpoiler.App
{
    public static class HtmlHelperExtensions
    {
        public static bool UserIsAuthenticated(this HtmlHelper @this)
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static bool UserIs(this HtmlHelper @this, UserRole role)
        {
            return @this.UserIsAuthenticated() && HttpContext.Current.User.IsInRole(role.ToString());
        }

        public static string ToJson(this HtmlHelper @this, object model)
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}