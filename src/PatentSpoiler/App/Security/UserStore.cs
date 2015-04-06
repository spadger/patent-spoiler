using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PatentSpoiler.App.Domain.Security;
using Raven.Client;

namespace PatentSpoiler.App.Security
{
    public class UserStore : IUserStore<PatentSpoilerUser>, IUserEmailStore<PatentSpoilerUser>, IUserPasswordStore<PatentSpoilerUser>
    {
        private readonly IAsyncDocumentSession session;

        public UserStore(IAsyncDocumentSession session)
        {
            this.session = session;
        }

        public void Dispose()
        {}

        public async Task CreateAsync(PatentSpoilerUser user)
        {
            await session.StoreAsync(user);
            await session.SaveChangesAsync();
        }

        public async Task UpdateAsync(PatentSpoilerUser user)
        {
            await session.StoreAsync(user);
            await session.SaveChangesAsync();
        }

        public async Task DeleteAsync(PatentSpoilerUser user)
        {
            session.Delete(user);
            await session.SaveChangesAsync();
        }

        public async Task<PatentSpoilerUser> FindByIdAsync(string userId)
        {
            return await session.LoadAsync<PatentSpoilerUser>(userId);
        }

        public async Task<PatentSpoilerUser> FindByNameAsync(string userName)
        {
            var user = await session.LoadAsync<PatentSpoilerUser>(userName);
            return user;
        }


        public async Task SetEmailAsync(PatentSpoilerUser user, string email)
        {
            await Task.Factory.StartNew(()=> user.Email = email);
        }

        public async Task<string> GetEmailAsync(PatentSpoilerUser user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(PatentSpoilerUser user)
        {
            return user.Roles.Contains(UserRole.EmailConfirmed);
        }

        public async Task SetEmailConfirmedAsync(PatentSpoilerUser user, bool confirmed)
        {
            if (confirmed)
            {
                user.Roles.Add(UserRole.EmailConfirmed);
            }
            else
            {
                user.Roles.Remove(UserRole.EmailConfirmed);
            }
        }

        public async Task<PatentSpoilerUser> FindByEmailAsync(string email)
        {
            return await session.Query<PatentSpoilerUser>().Where(x => x.Email == email).SingleOrDefaultAsync();
        }

        public async Task SetPasswordHashAsync(PatentSpoilerUser user, string passwordHash)
        {
            await Task.Factory.StartNew(() => user.Passwordhash = passwordHash);
        }

        public async Task<string> GetPasswordHashAsync(PatentSpoilerUser user)
        {
            return await Task.FromResult(user.Passwordhash);
        }

        public async Task<bool> HasPasswordAsync(PatentSpoilerUser user)
        {
            return await Task.FromResult(true);
        }
    }
}