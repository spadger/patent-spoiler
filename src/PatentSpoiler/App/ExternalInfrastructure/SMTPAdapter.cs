using System.Configuration;
using System.Net.Mail;

namespace PatentSpoiler.App.ExternalInfrastructure
{
    public interface ISMTPAdapter
    {
        void SendEmail(string from, string subject, string body, params string[] recipients);
    }

    public class SMTPAdapter : ISMTPAdapter
    {
        public void SendEmail(string from, string subject, string body, params string[] recipients)
        {
            var server = ConfigurationManager.ConnectionStrings["SMTP"].ConnectionString;

            var client = new SmtpClient(server);
            client.Send(from, string.Join("; ", recipients), subject, body);
        }
    }
}
