using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace JPProject.Sso.Infra.Identity.Services
{

    public class AuthEmailMessageSender : IEmailSender
    {
        private readonly ILogger<AuthEmailMessageSender> _logger;
        private readonly IEmailConfiguration _emailConfiguration;

        public AuthEmailMessageSender(IConfiguration config, ILogger<AuthEmailMessageSender> logger)
        {
            _logger = logger;
            _emailConfiguration = config.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (_emailConfiguration == null || !_emailConfiguration.SendEmail)
                return;

            var mimeMessage = new MimeMessage();
            mimeMessage.To.Add(new MailboxAddress(email));
            mimeMessage.From.Add(new MailboxAddress(_emailConfiguration.FromName, _emailConfiguration.FromAddress));

            mimeMessage.Subject = subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            mimeMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            _logger.LogInformation($"Sending e-mail to {email}");
            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, _emailConfiguration.UseSsl);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                await client.SendAsync(mimeMessage);
                client.Disconnect(true);
            }
            _logger.LogInformation($"E-mail to {email}: sent!");
        }
    }

    public class AuthSMSMessageSender : ISmsSender
    {
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
