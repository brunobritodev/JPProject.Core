using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.Validations.Email;

namespace JPProject.Sso.Domain.Commands.Email
{
    public class SaveEmailCommand : EmailCommand
    {

        public SaveEmailCommand(
            string content,
            Sender sender,
            string subject,
            EmailType type,
            BlindCarbonCopy bcc,
            string username)
        {
            Sender = sender;
            Content = content;
            Subject = subject;
            Type = type;
            Bcc = bcc;
            Username = username;
        }

        public override bool IsValid()
        {
            ValidationResult = new SaveEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public Models.Email ToModel()
        {
            return new Models.Email(Content, Subject, Sender, Type, Bcc);
        }
    }
}