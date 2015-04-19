using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using SendGrid;

namespace PatentSpoiler.App.ExternalInfrastructure.EmailVerification
{
    public class SendGridEmailVerificationMailAdapter : IEmailVerificationMailAdapter
    {
        public void Send(string email, string name, string link)
        {
            var username = ConfigurationManager.AppSettings["SendgridAccount"];
            var password = ConfigurationManager.AppSettings["SendgridPassword"];

            var mail = new SendGridMessage
            {
                From = new MailAddress("noreply@patent-spoiler.net", "Patent-Spoiler"),
                Subject = "Patent-Spoiler email-verification request",
                Html = " "
            };
            
            mail.AddTo(email);
            mail.EnableTemplateEngine("02f577ae-17ad-4b38-9d98-b740cb04e59f");
            mail.AddSubstitution("::name", new List<string>{ name });
            mail.AddSubstitution("::link", new List<string> { link });
            
            var credentials = new NetworkCredential(username, password);
            var transportWeb = new Web(credentials);
            transportWeb.Deliver(mail);
        }
    }
}