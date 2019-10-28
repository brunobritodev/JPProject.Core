using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class RemoveUserLoginCommand : UserLoginCommand
    {
        public RemoveUserLoginCommand(string username, string loginProvider, string providerKey)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveUserLoginCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}