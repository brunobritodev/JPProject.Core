using JPProject.Sso.Domain.Commands.Role;

namespace JPProject.Sso.Domain.Validations.Role
{
    public class RemoveRoleCommandValidation : RoleValidation<RemoveRoleCommand>
    {
        public RemoveRoleCommandValidation()
        {
            ValidateName();
        }
    }
}
