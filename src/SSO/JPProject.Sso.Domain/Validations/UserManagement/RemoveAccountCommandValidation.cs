using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class RemoveAccountCommandValidation : ProfileValidation<RemoveAccountCommand>
    {
        public RemoveAccountCommandValidation()
        {
            ValidateId();
        }
    }
}