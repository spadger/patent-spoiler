using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PatentSpoiler.App;
using PatentSpoiler.App.Commands;
using PatentSpoiler.App.Commands.User;
using PatentSpoiler.App.Data.Queries.Account;
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
        private readonly IFindUserForPasswordResetQuery findUserForPasswordResetQuery;
        private readonly IBeginPasswordResetCommand beginPasswordResetCommand;
        private readonly IConfirmForgottenPasswordCommand confirmForgottenPasswordCommand;

        public AccountController(IAuthenticationManager authenticationManager, IRegisterNewUserCommand registerNewUserCommand, PatentSpoilerUserManager userManager, IFindUserForPasswordResetQuery findUserForPasswordResetQuery, IBeginPasswordResetCommand beginPasswordResetCommand, IConfirmForgottenPasswordCommand confirmForgottenPasswordCommand)
        {
            this.authenticationManager = authenticationManager;
            this.registerNewUserCommand = registerNewUserCommand;
            this.userManager = userManager;
            this.findUserForPasswordResetQuery = findUserForPasswordResetQuery;
            this.beginPasswordResetCommand = beginPasswordResetCommand;
            this.confirmForgottenPasswordCommand = confirmForgottenPasswordCommand;
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

        [AllowAnonymous]
        public async Task<HttpStatusCodeResult> BeginForgottenPassword(string account)
        {
            await beginPasswordResetCommand.Execute(account);
            return new HttpStatusCodeResult(HttpStatusCode.Accepted);
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyResetToken(string token)
        {
            var tokenResult = await findUserForPasswordResetQuery.GetUserForResetTokenAsync(token);

            if (tokenResult.Item1 != PasswordResetTokenStatus.Valid)
            {
                ViewBag.Error = tokenResult.Item1;
                return View("TokenProblem");
            }

            ViewBag.token = token;
            return View();
        }

        [AllowAnonymous, HttpPut]
        public async Task<ActionResult> ConfirmForgottenPassword(PasswordResetViewModel model)
        {
            Debugger.Launch();
            var result = DomainResult.From(ModelState);

            if (result.HasErrors)
            {
                return this.JsonNetResult(result);
            }

            result = await confirmForgottenPasswordCommand.Execute(model);

            return this.JsonNetResult(result);
        }


        public async Task<ActionResult> Verify()
        {
            return Content("please verify your email address");
        }

        public ActionResult ResendVerificationEmail()
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public async Task<ActionResult> ConfirmEmailVerification()
        {
            return Content("Verified...");
        }
    }
}