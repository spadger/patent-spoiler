using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using PatentSpoiler.App.Domain.Security;
using Raven.Client;

namespace PatentSpoiler.App.Security
{
    public class PatentSpoilerUserManager : UserManager<PatentSpoilerUser>
    {
        public PatentSpoilerUserManager(IUserStore<PatentSpoilerUser> store) : base(store)
        {}

        public static PatentSpoilerUserManager Create(IdentityFactoryOptions<PatentSpoilerUserManager> options, IOwinContext context)
        {
            var manager = new PatentSpoilerUserManager(new UserStore(context.Get<IAsyncDocumentSession>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<PatentSpoilerUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            manager.PasswordHasher = new BCryptPasswordHasher();
            manager.EmailService = new EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<PatentSpoilerUser>(dataProtectionProvider.Create("Entropy for Patent Spoiler"));
            }
            return manager;
        }
    }
}