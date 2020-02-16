    using JPProject.Sso.Domain.Commands.UserManagement;

    namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class SetPasswordCommandValidation : PasswordCommandValidation<SetPasswordCommand>
    {
        public SetPasswordCommandValidation()
        {
            ValidateUsername();
            ValidatePassword();
        }
    }
}