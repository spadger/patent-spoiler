using System;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
using Raven.Client;
using Raven.Client.Linq;

namespace PatentSpoiler.App.Data.Queries.Account
{
    public interface IGetUserByTokenQuery
    {
        Task<PatentSpoilerUser> Get(Guid token);
    }

    public class GetUserByTokenQuery : IGetUserByTokenQuery
    {
        private readonly IAsyncDocumentSession _session;

        public GetUserByTokenQuery(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task<PatentSpoilerUser> Get(Guid token)
        {
            var user = await _session.Query<PatentSpoilerUser>()
                .Where(x => x.PasswordResetToken == token)
                .SingleOrDefaultAsync();

            return user;
        }
    }
}