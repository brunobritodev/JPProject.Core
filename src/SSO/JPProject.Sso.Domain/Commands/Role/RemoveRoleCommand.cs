using JPProject.Sso.Domain.Validations.Role;

namespace JPProject.Sso.Domain.Commands.Role
{
    public class RemoveRoleCommand : RoleCommand
    {
        public RemoveRoleCommand(string name)
        {
            Name = name;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
