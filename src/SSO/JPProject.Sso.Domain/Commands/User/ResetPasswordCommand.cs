using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class ResetPasswordCommand : UserCommand
    {
        public ResetPasswordCommand(string password, string confirmPassword, string code, string email)
        {
            Password = password;
            ConfirmPassword = confirmPassword;
            Code = code;
            Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = new ResetPasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}