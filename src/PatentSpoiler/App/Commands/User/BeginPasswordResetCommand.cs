using System.Linq;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.ExternalInfrastructure;
using PatentSpoiler.App.ExternalInfrastructure.PasswordReset;
using Raven.Client;
using Raven.Client.Linq;

namespace PatentSpoiler.App.Commands.User
{
    public interface IBeginPasswordResetCommand
    {
        Task Execute(string account);
    }

    public class BeginPasswordResetCommand : IBeginPasswordResetCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly IPasswordResetMailAdapter passwordResetMailAdapter;
        private readonly IProvideAppSettings _appSettings;

        public BeginPasswordResetCommand(IAsyncDocumentSession session, IPasswordResetMailAdapter passwordResetMailAdapter, IProvideAppSettings appSettings)
        {
            this.session = session;
            this.passwordResetMailAdapter = passwordResetMailAdapter;
            _appSettings = appSettings;
        }

        public async Task Execute(string account)
        {
            var user = await session.Query<PatentSpoilerUser>()
                .Where(x => x.Email == account || x.Id == account)
                .Take(1)
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return;
            }

            var resetToken = user.NewPasswordResetToken();
            
            await session.StoreAsync(user);
            await session.SaveChangesAsync();

            passwordResetMailAdapter.Send(user.Email, user.Email, _appSettings["SiteRoot"] + "/account/VerifyResetToken?token=" + resetToken);
        }
    }
}