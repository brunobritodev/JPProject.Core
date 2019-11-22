using JPProject.Sso.Domain.Commands.Email;

namespace JPProject.Sso.Domain.Validations.Email
{
    public class SaveEmailCommandValidation : EmailValidation<SaveEmailCommand>
    {
        public SaveEmailCommandValidation()
        {
            ValidateSubject();
        }
    }
}