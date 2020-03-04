using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class ChangePasswordCommandValidation : PasswordCommandValidation<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidation()
        {
            ValidateUsername();
            ValidateOldPassword();
            ValidatePassword();
        }
    }
}