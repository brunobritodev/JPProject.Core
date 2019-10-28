using JPProject.Sso.Domain.Commands.Role;

namespace JPProject.Sso.Domain.Validations.Role
{
    public class SaveRoleCommandValidation : RoleValidation<SaveRoleCommand>
    {
        public SaveRoleCommandValidation()
        {
            ValidateName();
        }
    }
}