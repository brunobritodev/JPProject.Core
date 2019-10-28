using JPProject.Sso.Domain.Validations.Role;

namespace JPProject.Sso.Domain.Commands.Role
{
    public class RemoveUserFromRoleCommand : RoleCommand
    {

        public RemoveUserFromRoleCommand(string roleName, string username)
        {
            Username = username;
            Name = roleName;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveUserFromRoleValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}