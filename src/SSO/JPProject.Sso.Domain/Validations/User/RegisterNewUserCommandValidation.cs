using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RegisterNewUserCommandValidation : UserValidation<UserCommand>
    {
        public RegisterNewUserCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidatePassword();

        }
    }
}
