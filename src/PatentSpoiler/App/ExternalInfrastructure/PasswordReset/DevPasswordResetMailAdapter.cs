using System.Net.Mail;

namespace PatentSpoiler.App.ExternalInfrastructure.PasswordReset
{
    public class DevPasswordResetMailAdapter : IPasswordResetMailAdapter
    {
        public void Send(string email, string name, string link)
        {
            var client = new SmtpClient("127.0.0.1");
            client.Send("test@test.test", email, "reset", string.Format("name:{0}, link:{1}", name, link));
        }
    }
}
