using JPProject.Sso.Domain.Commands.Role;

namespace JPProject.Sso.Domain.Validations.Role
{
    public class UpdateRoleCommandValidation : RoleValidation<UpdateRoleCommand>
    {
        public UpdateRoleCommandValidation()
        {
            ValidateName();
            ValidateNewName();
        }
    }
}