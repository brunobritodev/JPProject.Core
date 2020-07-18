using JPProject.Admin.Domain.Validations.ApiResource;

namespace JPProject.Admin.Domain.Commands.ApiScope
{
    public class UpdateApiScopeCommand : ApiScopeCommand
    {


        public UpdateApiScopeCommand(string oldName, IdentityServer4.Models.ApiScope apiScope)
        {
            ApiScope = apiScope;
            OldName = oldName;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApiScopeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public IdentityServer4.Models.ApiScope ToModel()
        {
            return ApiScope;
        }
    }
}