using FluentValidation;
using JPProject.Sso.Domain.Commands.Email;

namespace JPProject.Sso.Domain.Validations.Email
{
    public abstract class EmailValidation<T> : AbstractValidator<T> where T : EmailCommand
    {

        protected void ValidateBcc()
        {
            RuleFor(c => c.Bcc)
                .NotEmpty();
        }

        protected void ValidateSubject()
        {
            RuleFor(c => c.Subject)
                .NotEmpty();
        }

        protected void ValidateSendAddress()
        {
            RuleFor(c => c.Sender.Address)
                .NotEmpty().EmailAddress();
        }
        protected void ValidateSendName()
        {
            RuleFor(c => c.Sender.Name)
                .NotEmpty();
        }
        protected void ValidateDescription()
        {
            RuleFor(c => c.Description)
                .NotEmpty();
        }

        protected void ValidateUsername()
        {
            RuleFor(c => c.Username)
                .NotEmpty();
        }
    }
}