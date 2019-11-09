using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RegisterNewUserCommandValidation : UserValidation<RegisterNewUserCommand>
    {
        public RegisterNewUserCommandValidation(RegisterNewUserCommand registerNewUserCommand)
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidatePassword();
            if (registerNewUserCommand.Birthdate.HasValue)
                ValidateBirthdate();
        }
    }
}
