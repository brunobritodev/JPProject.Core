using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RemoveUserLoginCommandValidation : UserLoginValidation<RemoveUserLoginCommand>
    {
        public RemoveUserLoginCommandValidation()
        {
            ValidateUsername();
            ValidateLoginProvider();
            ValidateProviderKey();
        }
    }
}