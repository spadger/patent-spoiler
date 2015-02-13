using System;
using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Data.Indexes.User;
using PatentSpoiler.App.Domain.Security;
using Raven.Client;

namespace PatentSpoiler.App.Data.Queries.Account
{
    public interface IFindUserForPasswordResetQuery
    {
        Task<Tuple<PasswordResetTokenStatus, PatentSpoilerUser>> GetUserForResetTokenAsync(string passwordResetTokenString);
    }

    public class FindUserForPasswordResetQuery : IFindUserForPasswordResetQuery
    {
        private readonly IAsyncDocumentSession session;

        public FindUserForPasswordResetQuery(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public async Task<Tuple<PasswordResetTokenStatus, PatentSpoilerUser>> GetUserForResetTokenAsync(string passwordResetTokenString)
        {
            Guid passwordResetToken;
            if(!Guid.TryParse(passwordResetTokenString, out passwordResetToken))
            {
                return Tuple.Create(PasswordResetTokenStatus.TokenFormatNotValid, (PatentSpoilerUser)null);
            }

            var user = await session.Query<PatentSpoilerUser, PatentSpoilerUserIndex>()
                .Where(x => x.PasswordResetToken == passwordResetToken)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return Tuple.Create(PasswordResetTokenStatus.UserNotFound, user);
            }

            if (user.PasswordResetTokenExpiry < DateTime.Now)
            {
                return Tuple.Create(PasswordResetTokenStatus.TokenExpired, user);
            }


            return Tuple.Create(PasswordResetTokenStatus.Valid, user);
        }
    }

    public enum PasswordResetTokenStatus
    {
        UserNotFound,
        TokenFormatNotValid,
        TokenExpired,
        Valid
    }
}
