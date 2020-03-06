using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Models;
using JPProject.Domain.Core.Util;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.ViewModels;
using JPProject.Sso.Domain.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace JPProject.Sso.Domain.Models
{
    public class Email : Entity
    {
        public Email() { }
        public Email(string content, string subject, Sender sender, EmailType type, BlindCarbonCopy bcc = null)
        {
            Id = Guid.NewGuid();
            Content = content;
            Sender = sender;
            Subject = subject;
            Type = type;
            Bcc = bcc;
        }
        public EmailType Type { get; private set; }
        public string Content { get; private set; }
        public string Subject { get; private set; }
        public Sender Sender { get; private set; }
        public BlindCarbonCopy Bcc { get; private set; }
        public string UserName { get; protected set; }
        public DateTime Updated { get; private set; } = DateTime.UtcNow;

        public void Update(SaveEmailCommand request)
        {
            Subject = request.Subject;
            Content = request.Content;

            if (request.Bcc != null && request.Bcc.IsValid())
                Bcc = request.Bcc;

            if (request.Sender != null && request.Sender.IsValid())
                Sender = request.Sender;

            UserName = request.Username;
            Updated = DateTime.UtcNow;
        }

        public EmailMessage GetMessage(IDomainUser user, AccountResult created, UserCommand command,
            IEnumerable<Claim> claims)
        {
            return new EmailMessage(
                user.Email,
                Bcc,
                GetFormatedContent(Subject, user, created, command, claims),
                GetFormatedContent(Content, user, created, command, claims),
                Sender);
        }

        private string GetFormatedContent(string content, IDomainUser user, AccountResult created, UserCommand command,
            IEnumerable<Claim> claims)
        {
            if (content is null)
                return string.Empty;

            return content
                .Replace("{{picture}}", claims.ValueOf(JwtClaimTypes.Picture))
                .Replace("{{name}}", claims.ValueOf(JwtClaimTypes.Name, JwtClaimTypes.PreferredUserName, JwtClaimTypes.GivenName))
                .Replace("{{username}}", user.UserName)
                .Replace("{{code}}", created.Code)
                .Replace("{{url}}", created.Url)
                .Replace("{{provider}}", command.Provider)
                .Replace("{{phoneNumber}}", user.PhoneNumber)
                .Replace("{{email}}", user.Email);
        }
    }
}
