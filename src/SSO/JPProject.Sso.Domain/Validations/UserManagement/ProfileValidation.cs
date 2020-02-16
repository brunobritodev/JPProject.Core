using FluentValidation;
using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class ProfileValidation<T> : AbstractValidator<T> where T : ProfileCommand
    {

        protected void ValidateName()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please ensure you have entered the Username")
                .Length(2, 150).WithMessage("The Username must have between 2 and 150 characters");
        }

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
