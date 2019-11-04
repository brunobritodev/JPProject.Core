using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class ConfirmEmailCommand : UserCommand
    {

        public ConfirmEmailCommand(string code, string email)
        {
            Code = code;
            Email = email;
        }

        public override bool IsValid()
        {
            ValidationResult = new ConfirmEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}