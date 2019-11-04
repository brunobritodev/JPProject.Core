using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class SendResetLinkCommandValidation : UserValidation<SendResetLinkCommand>
    {
        public SendResetLinkCommandValidation()
        {
            ValidateUsername();
            ValidateEmail();
        }
    }
}