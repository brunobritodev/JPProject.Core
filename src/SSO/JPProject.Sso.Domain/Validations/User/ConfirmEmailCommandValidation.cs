using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class ConfirmEmailCommandValidation : UserValidation<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidation()
        {
            ValidateEmail();
            ValidateCode();
        }
    }
}