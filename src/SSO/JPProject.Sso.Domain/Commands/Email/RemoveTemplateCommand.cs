using JPProject.Sso.Domain.Validations.Email;

namespace JPProject.Sso.Domain.Commands.Email
{
    public class RemoveTemplateCommand : TemplateCommand
    {

        public RemoveTemplateCommand(string name)
        {
            Name = name.Trim();
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveTemplateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}