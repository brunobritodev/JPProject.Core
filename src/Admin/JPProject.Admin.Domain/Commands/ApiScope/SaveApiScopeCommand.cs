using JPProject.Admin.Domain.Validations.ApiResource;

namespace JPProject.Admin.Domain.Commands.ApiScope
{
    public class SaveApiScopeCommand : ApiScopeCommand
    {
        public SaveApiScopeCommand(IdentityServer4.Models.ApiScope apiScope)
        {
            ApiScope = apiScope;
        }

        public override bool IsValid()
        {
            ValidationResult = new SaveApiScopeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public IdentityServer4.Models.ApiScope ToModel()
        {
            ApiScope.Enabled = true;
            return ApiScope;
        }
    }
}