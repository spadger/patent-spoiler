using System.Security.Claims;
using System.Threading.Tasks;
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

        public override async Task<ClaimsIdentity> CreateIdentityAsync(PatentSpoilerUser user, string authenticationType)
        {
            var identity =  await base.CreateIdentityAsync(user, authenticationType);
            
            if (user.Roles != null)
            {
                foreach (var role in user.Roles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
                }
            }

            return identity;
        }

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