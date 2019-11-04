using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class SaveUserRoleCommand : UserRoleCommand
    {

        public SaveUserRoleCommand(string username, string role)
        {
            Role = role;

            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new SaveUserRoleCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}