using System;
using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.ExternalInfrastructure;
using Raven.Client;
using Raven.Client.Linq;

namespace PatentSpoiler.App.Commands.User
{
    public interface ICompleteConfirmEmailCommand
    {
        Task<DomainResult> ExecuteAsync(string email, string confirmationToken);
    }

    public class CompleteConfirmEmailCommand : ICompleteConfirmEmailCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IProvideAppSettings appSettings;

        public CompleteConfirmEmailCommand(IAsyncDocumentSession session, IProvideAppSettings appSettings)
        {
            this.session = session;
            this.appSettings = appSettings;
        }

        public async Task<DomainResult> ExecuteAsync(string email, string confirmationToken)
        {
            var user = await session.Query<PatentSpoilerUser>()
                .Where(x => x.Email == email)
                .Take(1)
                .SingleOrDefaultAsync();

            var result = new DomainResult();

            if (user == null)
            {
                result.AddGeneralError("User does not exist");
                return result;
            }

            var matches = false;

            try
            {
                var confirmationString = string.Format("{0}:{1}", email, appSettings["EmailVerificationSecret"]);
                matches = BCrypt.Net.BCrypt.Verify(confirmationString, confirmationToken);
            }
            catch (Exception){}

            if (!matches)
            {
                result.AddGeneralError("Your token was not valid");
                return result;
            }

            user.Roles.Add(UserRole.EmailConfirmed);
            
            await session.StoreAsync(user);
            await session.SaveChangesAsync();

            return result;
        }
    }
}