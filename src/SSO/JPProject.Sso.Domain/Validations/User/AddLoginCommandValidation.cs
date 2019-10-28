using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class AddLoginCommandValidation : UserValidation<AddLoginCommand>
    {
        public AddLoginCommandValidation()
        {
            ValidateEmail();
            ValidateProvider();
            ValidateProviderId();
        }

    }
}