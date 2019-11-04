using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RegisterNewUserWithProviderCommandValidation : UserValidation<RegisterNewUserWithProviderCommand>
    {
        public RegisterNewUserWithProviderCommandValidation()
        {
            ValidateName();
            ValidateUsername();
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }
    }
}