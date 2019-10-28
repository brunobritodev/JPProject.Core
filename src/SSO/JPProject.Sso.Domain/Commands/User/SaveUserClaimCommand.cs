using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class SaveUserClaimCommand : UserClaimCommand
    {
        public SaveUserClaimCommand(string username, string type, string value)
        {
            Type = type;
            Username = username;
            Value = value;
        }
        public override bool IsValid()
        {
            ValidationResult = new SaveUserClaimCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}