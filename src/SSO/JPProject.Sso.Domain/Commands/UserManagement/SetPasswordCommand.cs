using JPProject.Sso.Domain.Validations.UserManagement;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public class SetPasswordCommand : PasswordCommand
    {
        public SetPasswordCommand(string username, string newPassword, string confirmPassword)
        {
            Username = username;
            Password = newPassword;
            ConfirmPassword = confirmPassword;
        }

        public override bool IsValid()
        {
            ValidationResult = new SetPasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}