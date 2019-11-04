using JPProject.Sso.Domain.Validations.Role;

namespace JPProject.Sso.Domain.Commands.Role
{
    public class UpdateRoleCommand : RoleCommand
    {

        public UpdateRoleCommand(string name, string oldName)
        {
            OldName = oldName;
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}