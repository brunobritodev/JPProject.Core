using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class RemoveUserClaimCommandValidation : UserClaimValidation<RemoveUserClaimCommand>
    {
        public RemoveUserClaimCommandValidation()
        {
            ValidateUsername();
            ValidateKey();
        }
    }
}