using JPProject.Domain.Core.Models;
using JPProject.Sso.Domain.Commands.Email;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.ViewModels.User;
using System;

namespace JPProject.Sso.Domain.Models
{
    public class Email : Entity
    {
        public Email() { }
        public Email(string content, string subject, Sender sender, EmailType type, BlindCarbonCopy bcc)
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
            Bcc = request.Bcc;
            UserName = request.Username;
            Updated = DateTime.UtcNow;
        }

        public EmailMessage GetMessage(User user, AccountResult created, UserCommand command)
        {
            return new EmailMessage(
                user.Email,
                Bcc,
                GetFormatedContent(Subject, user, created, command),
                GetFormatedContent(Content, user, created, command),
                Sender);
        }


        private string GetFormatedContent(string content, User user, AccountResult created, UserCommand command)
        {
            return content
                .Replace("{{picture}}", user.Picture)
                .Replace("{{name}}", user.Name)
                .Replace("{{username}}", user.UserName)
                .Replace("{{code}}", created.Code)
                .Replace("{{url}}", created.Url)
                .Replace("{{provider}}", command.Provider)
                .Replace("{{phoneNumber}}", user.PhoneNumber)
                .Replace("{{email}}", user.Email);
        }
    }
}
