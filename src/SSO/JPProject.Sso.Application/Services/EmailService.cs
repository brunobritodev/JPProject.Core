using System.Threading.Tasks;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Infra.Identity.Extensions;
using JPProject.Sso.Infra.Identity.Services;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;

namespace JPProject.Sso.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IEmailConfiguration _emailConfiguration;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _logger = logger;
            _emailConfiguration = config.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            if (_emailConfiguration == null || !_emailConfiguration.SendEmail)
                return;

            var mimeMessage = new MimeMessage();
            mimeMessage.To.Add(new MailboxAddress(message.Email));
            mimeMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.FromAddress));

            if (message.Bcc.IsValid())
                mimeMessage.To.AddRange(message.Bcc.Recipients.ToMailboxAddress());

            mimeMessage.Subject = message.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            _logger.LogInformation($"Sending e-mail to {message.Email}");
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, _emailConfiguration.UseSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                await client.SendAsync(mimeMessage);
                client.Disconnect(true);
            }
            _logger.LogInformation($"E-mail to {message.Email}: sent!");
        }
    }
}