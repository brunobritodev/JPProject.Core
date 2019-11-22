using JPProject.Sso.Domain.Commands.Email;

namespace JPProject.Sso.Domain.Validations.Email
{
    public class AddTemplateCommandValidation : TemplateValidation<SaveTemplateCommand>
    {
        public AddTemplateCommandValidation()
        {
            ValidateName();
            ValidateContent();
        }
    }
}