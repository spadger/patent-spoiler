using System.Linq;
using PatentSpoiler.App.Domain.Security;
using Raven.Client.Indexes;

namespace PatentSpoiler.App.Data.Indexes.User
{
    public class PatentSpoilerUserIndex : AbstractIndexCreationTask<PatentSpoilerUser>
    {
        public PatentSpoilerUserIndex()
        {
            Map = users =>  from user in users
                            select new { user.Email, user.PasswordResetToken, user.Id };
        }
    }
}