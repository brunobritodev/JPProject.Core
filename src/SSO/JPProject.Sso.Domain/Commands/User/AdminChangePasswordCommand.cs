using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class AdminChangePasswordCommand : UserCommand
    {
        public AdminChangePasswordCommand(string password, string changePassword, string username)
        {
            Username = username;
            Password = password;
            ConfirmPassword = changePassword;
        }
        public override bool IsValid()
        {
            ValidationResult = new AdminChangePasswordCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}