using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace PatentSpoiler.App.Security
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await Task.Run(()=>new SmtpClient().Send("info@patentspoiler.net", message.Destination, message.Subject, message.Body));
        }
    }
}