using JPProject.Sso.Domain.Commands.User;

namespace JPProject.Sso.Domain.Validations.User
{
    public class SynchronizeClaimsCommandValidation : UserClaimValidation<SynchronizeClaimsCommand>
    {
        public SynchronizeClaimsCommandValidation()
        {
            ValidateUsername();
            ValidateClaims();
        }
    }
}