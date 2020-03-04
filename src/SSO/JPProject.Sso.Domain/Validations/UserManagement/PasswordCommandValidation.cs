using FluentValidation;
using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class PasswordCommandValidation<T> : AbstractValidator<T> where T : PasswordCommand
    {
        protected void ValidateOldPassword()
        {
            RuleFor(c => c.OldPassword)
                .NotEmpty().WithMessage("Please ensure you have entered the Old Password")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");
        }

        protected void ValidatePassword()
        {
            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please ensure you have entered the password")
                .Equal(c => c.ConfirmPassword).WithMessage("Password and Confirm password must be equal")
                .MinimumLength(8).WithMessage("Password minimun length must be 8 characters");
        }

        protected void ValidateUsername()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Invalid user");
        }
    }
}