using JPProject.Sso.Domain.Commands.Role;

namespace JPProject.Sso.Domain.Validations.Role
{
    public class RemoveUserFromRoleValidation : RoleValidation<RemoveUserFromRoleCommand>
    {
        public RemoveUserFromRoleValidation()
        {
            ValidateName();
            ValidateUsername();
        }
    }
}