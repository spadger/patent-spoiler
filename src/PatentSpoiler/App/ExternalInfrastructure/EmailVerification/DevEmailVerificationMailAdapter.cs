using System.Net.Mail;

namespace PatentSpoiler.App.ExternalInfrastructure.EmailVerification
{
    public class DevEmailVerificationMailAdapter : IEmailVerificationMailAdapter
    {
        public void Send(string email, string name, string link)
        {
            var client = new SmtpClient("127.0.0.1");
            client.Send("test@test.test", email, "Verify", string.Format("name:{0}, link:{1}", name, link));
        }
    }
}
