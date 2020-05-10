using FluentValidation;
using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public abstract class UserClaimValidation<T> : AbstractValidator<T> where T : UserClaimCommand
    {

        protected void ValidateUsername()
        {
            RuleFor(c => c.Username).NotEmpty().WithMessage("Username must be set");
        }

        protected void ValidateValue()
        {
            RuleFor(c => c.Value).NotEmpty().WithMessage("Secret must be set");
        }
        protected void ValidateKey()
        {
            RuleFor(c => c.Type).NotEmpty().WithMessage("Please ensure you have entered key");
        }

        protected void ValidateClaims()
        {
            RuleFor(c => c.Claims).NotEmpty().WithMessage("Please provide at least one claim");
        }
    }
}