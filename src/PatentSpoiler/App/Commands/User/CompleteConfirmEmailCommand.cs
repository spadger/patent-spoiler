using System;
using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
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

        public CompleteConfirmEmailCommand(IAsyncDocumentSession session)
        {
            this.session = session;
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
                matches = BCrypt.Net.BCrypt.Verify(email, confirmationToken);
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