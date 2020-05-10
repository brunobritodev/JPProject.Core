using System.Collections.Generic;
using System.Security.Claims;
using JPProject.Sso.Domain.Validations.User;

namespace JPProject.Sso.Domain.Commands.User
{
    public class SynchronizeClaimsCommand : UserClaimCommand
    {

        public SynchronizeClaimsCommand(string username, IEnumerable<Claim> claims)
        {
            Claims = claims;
            Username = username;
        }
        public override bool IsValid()
        {
            ValidationResult = new SynchronizeClaimsCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}