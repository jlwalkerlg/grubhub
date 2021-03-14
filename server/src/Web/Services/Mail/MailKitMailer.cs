using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Web.Services.Mail
{
    public class MailKitMailer : IMailer
    {
        private readonly MailSettings settings;

        public MailKitMailer(MailSettings settings)
        {
            this.settings = settings;
        }

        public async Task Send(Mail mail, CancellationToken cancellationToken = default)
        {
            var builder = new BodyBuilder()
            {
                HtmlBody = mail.Body,
            };

            var message = new MimeMessage()
            {
                Subject = mail.Subject,
                From = {new MailboxAddress(mail.FromName, mail.FromAddress)},
                To = {new MailboxAddress(mail.ToName, mail.ToAddress)},
                Body = builder.ToMessageBody(),
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                settings.Host,
                settings.Port,
                SecureSocketOptions.StartTls,
                cancellationToken);

            await smtp.AuthenticateAsync(
                settings.Username,
                settings.Password,
                cancellationToken);

            await smtp.SendAsync(message, cancellationToken);

            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
