using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class RemoveUserRoleCommand : UserRoleCommand
    {

        public RemoveUserRoleCommand(string username, string role)
        {
            Role = role;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new RemoveUserRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}