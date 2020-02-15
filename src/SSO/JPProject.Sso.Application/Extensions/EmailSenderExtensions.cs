using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace JPProject.Sso.Application.Extensions
{
    public static class EmailSenderExtensions
    {
        public static IEnumerable<MailboxAddress> ToMailboxAddress(this string[] recipients)
        {
            return recipients.Select(s => new MailboxAddress(s));
            //return emailService.SendEmailAsync(email, "Confirm your email",
            //    $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
