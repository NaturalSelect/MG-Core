using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;
using System.Net;

namespace MG_Core.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender() {
            IdentitySetting setting = new IdentitySetting();
            smtp = new SmtpClient()
            { UseDefaultCredentials = true,
                Host = setting.GetEmailServer(),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential() {
                    UserName = setting.GetEmailAdress(),
                    Password = setting.GetEmailUserPassword()
                },
                Port = setting.GetSMTPPort(),
                EnableSsl = setting.SMTPIsSSL(),
            };
            Mail = new MailMessage() { From = new MailAddress(setting.GetEmailAdress()), BodyEncoding = Encoding.UTF8, SubjectEncoding = Encoding.UTF8 };
        }
        private readonly MailMessage Mail;
        private readonly SmtpClient smtp ;
        public Task SendEmailAsync(string email, string subject, string message)
        {

            Mail.To.Add(email);
            Mail.Priority = MailPriority.Normal;
            Mail.Subject = subject;
            Mail.Body = message;
            Mail.IsBodyHtml = true;
            Mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            smtp.Send(Mail);
            return Task.CompletedTask;
        }
    }
}
