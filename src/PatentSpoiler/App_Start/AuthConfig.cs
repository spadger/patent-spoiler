using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Ninject;
using Ninject.Web.Common;
using Owin;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.Security;
using Raven.Client;

[assembly: OwinStartupAttribute(typeof(PatentSpoiler.AuthConfig), "Setup")]
namespace PatentSpoiler
{
    public class AuthConfig
    {
        public void Setup(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(() => new Bootstrapper().Kernel.Get<IDocumentSession>());
            app.CreatePerOwinContext(() => new Bootstrapper().Kernel.Get<IAsyncDocumentSession>());
            //app.CreatePerOwinContext(()=>);
            app.CreatePerOwinContext<PatentSpoilerUserManager>(PatentSpoilerUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<PatentSpoilerUserManager, PatentSpoilerUser>(
                        validateInterval: TimeSpan.FromMinutes(300),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
}