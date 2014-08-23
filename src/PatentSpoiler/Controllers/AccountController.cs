using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Security;

namespace PatentSpoiler.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private IAuthenticationManager authenticationManager;
        private PatentSpoilerUserManager userManager;

        public AccountController(IAuthenticationManager authenticationManager, PatentSpoilerUserManager userManager)
        {
            this.authenticationManager = authenticationManager;
            this.userManager = userManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindAsync(model.Username, model.Password);
                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = model.ToUser();
                IdentityResult result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await SignInAsync(user, isPersistent: model.RememberMe);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
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

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}