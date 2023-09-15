using IdentityServer.Configs;
using IdentityServer.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IdentityServer.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger _logger;
        private readonly MailSettings _mailSettings;
        public EmailService(IOptions<MailSettings> mailSettings,
                           ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }


        public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellation)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail),
                Subject = mailRequest.Subject,
            };
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellation);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password, cancellation);
            await smtp.SendAsync(email, cancellation);
            smtp.Disconnect(true, cancellation);
        }
    }
}
