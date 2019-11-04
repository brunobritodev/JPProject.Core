using FluentValidation;
using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class ResetPasswordCommandValidation : UserValidation<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidation()
        {
            ValidateEmail();
            ValidatePassword();
            ValidateCode();
        }

        protected void ValidateCode()
        {
            RuleFor(c => c.Code)
                .NotEmpty();
        }

    }
}