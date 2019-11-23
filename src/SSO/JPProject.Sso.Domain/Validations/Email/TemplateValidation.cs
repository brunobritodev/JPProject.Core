using FluentValidation;
using JPProject.Sso.Domain.Commands.Email;
using System;

namespace JPProject.Sso.Domain.Validations.Email
{
    public abstract class TemplateValidation<T> : AbstractValidator<T> where T : TemplateCommand
    {

        protected void ValidateContent()
        {
            RuleFor(c => c.Content)
                .NotEmpty();
        }

        protected void ValidateSubject()
        {
            RuleFor(c => c.Subject)
                .NotEmpty();
        }

        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .Must(a => !a.Contains(" ") && Uri.IsWellFormedUriString(a, UriKind.RelativeOrAbsolute));
        }
        protected void ValidateOldName()
        {
            RuleFor(c => c.OldName)
                .NotEmpty();
        }

    }
}
