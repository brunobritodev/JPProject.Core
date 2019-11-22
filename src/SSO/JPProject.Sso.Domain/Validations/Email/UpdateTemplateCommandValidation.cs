using JPProject.Sso.Domain.Commands.Email;

namespace JPProject.Sso.Domain.Validations.Email
{
    public class UpdateTemplateCommandValidation : TemplateValidation<UpdateTemplateCommand>
    {
        public UpdateTemplateCommandValidation()
        {
            ValidateOldName();
            ValidateName();
            ValidateSubject();
            ValidateContent();
        }
    }
}