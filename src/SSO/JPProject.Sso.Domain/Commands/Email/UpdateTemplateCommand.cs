using JPProject.Sso.Domain.Validations.Email;

namespace JPProject.Sso.Domain.Commands.Email
{
    public class UpdateTemplateCommand : TemplateCommand
    {

        public UpdateTemplateCommand(
            string oldname,
            string subject,
            string content,
            string name,
            string userName)
        {
            OldName = oldname;
            UserName = userName;
            Subject = subject;
            Content = content;
            Name = name.Trim();
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateTemplateCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

    }
}