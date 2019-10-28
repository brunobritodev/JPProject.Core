using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class SaveUserClaimCommandValidation : UserClaimValidation<SaveUserClaimCommand>
    {
        public SaveUserClaimCommandValidation()
        {
            ValidateUsername();
            ValidateKey();
            ValidateValue();
        }
    }
}