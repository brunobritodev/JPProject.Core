using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RegisterNewUserWithoutPassCommandValidation : UserValidation<RegisterNewUserWithoutPassCommand>
    {
        public RegisterNewUserWithoutPassCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }

        

    }
}