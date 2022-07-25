/*
using MailKit.Net.Smtp;
using MimeKit;

namespace CarbonaraWebAPI.Services
{
    public class MailService
    {

        public MailService()
        {
            
        }

        public void SendRegistrationMail()
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("server@car-bonara.de", "server@car-bonara.de"));
            mailMessage.To.Add(new MailboxAddress("jonehlers@mailbox.org", "jonehlers@mailbox.org"));
            mailMessage.Subject = "subject";
            mailMessage.Body = new TextPart("plain")
            {
                Text = "Hello"
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.ionos.de", 465, true);
                smtpClient.Authenticate("server@car-bonara.de", "ae3wOORGCURZXTq6IgFE");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
*/