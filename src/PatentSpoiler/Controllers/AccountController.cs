using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PatentSpoiler.App;
using PatentSpoiler.App.Commands;
using PatentSpoiler.App.Commands.User;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IAuthenticationManager authenticationManager;
        private readonly IRegisterNewUserCommand registerNewUserCommand;
        private readonly PatentSpoilerUserManager userManager;

        public AccountController(IAuthenticationManager authenticationManager, IRegisterNewUserCommand registerNewUserCommand, PatentSpoilerUserManager userManager)
        {
            this.authenticationManager = authenticationManager;
            this.registerNewUserCommand = registerNewUserCommand;
            this.userManager = userManager;
        }
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var result = new DomainResult();

            if (!ModelState.IsValid)
            {
                result.AddError(ModelState);
                return this.JsonNetResult(result);
            }

            var user = await userManager.FindAsync(model.Username, model.Password);
            if (user != null)
            {
                await SignInAsync(user, model.RememberMe);
            }
            else
            {
                result.AddGeneralError("Invalid username or password");
            }

            return this.JsonNetResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegistrationViewModel model)
        {
            var result = await registerNewUserCommand.RegisterAsync(model);
            if (result.Success)
            {
                await SignInAsync(registerNewUserCommand.User, isPersistent: model.RememberMe);
            }

            return this.JsonNetResult(result);
        }

        private async Task SignInAsync(PatentSpoilerUser user, bool isPersistent)
        {
            DoLogout();

            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(
               new AuthenticationProperties
               {
                   IsPersistent = isPersistent,
                   ExpiresUtc = DateTimeOffset.UtcNow.AddDays(28)

               }, identity);
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            DoLogout();
            return View();
        }

        private void DoLogout()
        {
            authenticationManager.SignOut(new[] { DefaultAuthenticationTypes.ApplicationCookie });
        }
    }
}