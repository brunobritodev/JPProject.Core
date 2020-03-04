using FluentValidation;
using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class ProfileValidation<T> : AbstractValidator<T> where T : ProfileCommand
    {
        protected void ValidateUsername()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Invalid user");
        }

        protected void ValidatePicture()
        {
            RuleFor(c => c.Picture)
                .NotEmpty().WithMessage("Please ensure you have entered the picture");
        }
    }
}
