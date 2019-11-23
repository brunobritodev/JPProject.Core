using JPProject.Sso.Domain.Commands.Email;

namespace JPProject.Sso.Domain.Validations.Email
{
    public class RemoveTemplateCommandValidation : TemplateValidation<RemoveTemplateCommand>
    {
        public RemoveTemplateCommandValidation()
        {
            ValidateName();
        }
    }
}