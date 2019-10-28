using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RemoveUserRoleCommandValidation : UserRoleValidation<RemoveUserRoleCommand>
    {
        public RemoveUserRoleCommandValidation()
        {
            ValidateUsername();
            ValidateRole();
        }
    }
}