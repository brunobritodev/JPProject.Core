using JPProject.Admin.Domain.Commands.ApiResource;
using JPProject.Admin.Domain.Commands.ApiScope;

namespace JPProject.Admin.Domain.Validations.ApiResource
{
    public class SaveApiScopeCommandValidation : ApiScopeValidation<ApiScopeCommand>
    {
        public SaveApiScopeCommandValidation()
        {
            ValidateScopeName();
        }
    } 
    public class UpdateApiScopeCommandValidation : ApiScopeValidation<ApiScopeCommand>
    {
        public UpdateApiScopeCommandValidation()
        {
            ValidateScopeName();
            ValidateOldName();
        }
    }
}