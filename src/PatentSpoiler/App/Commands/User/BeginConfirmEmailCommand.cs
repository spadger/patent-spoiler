using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.ExternalInfrastructure;
using PatentSpoiler.App.ExternalInfrastructure.EmailVerification;
using Raven.Client;
using Raven.Client.Linq;

namespace PatentSpoiler.App.Commands.User
{
    public interface IBeginConfirmEmailCommand
    {
        Task<DomainResult> ExecuteAsync(string account);
    }

    public class BeginConfirmEmailCommand : IBeginConfirmEmailCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IEmailVerificationMailAdapter emailVerificationMailAdapter;
        private readonly IProvideAppSettings appSettings;

        public BeginConfirmEmailCommand(IAsyncDocumentSession session, IEmailVerificationMailAdapter emailVerificationMailAdapter, IProvideAppSettings appSettings)
        {
            this.session = session;
            this.emailVerificationMailAdapter = emailVerificationMailAdapter;
            this.appSettings = appSettings;
        }

        public async Task<DomainResult> ExecuteAsync(string account)
        {
            var user = await session.Query<PatentSpoilerUser>()
                .Where(x => x.Email == account)
                .Take(1)
                .SingleOrDefaultAsync();

            var result = new DomainResult();

            if (user == null)
            {
                result.AddGeneralError("User does not exist");
                return result;
            }

            var confirmationToken = BCrypt.Net.BCrypt.HashPassword(string.Format("{0}:{1}", user.Email, appSettings["EmailVerificationSecret"]));
            var link = string.Format("{0}//account/ConfirmEmail?email={1}&confirmationToken={2}", appSettings["SiteRoot"], user.Email, confirmationToken);

            emailVerificationMailAdapter.Send(user.Email, user.Email, link);

            return result;
        }
    }
}