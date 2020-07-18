using JPProject.Admin.Domain.Validations.ApiResource;

namespace JPProject.Admin.Domain.Commands.ApiScope
{
    public class RemoveApiScopeCommand : ApiScopeCommand
    {

        public RemoveApiScopeCommand(string name)
        {
            ResourceName = name;
        }


        public override bool IsValid()
        {
            ValidationResult = new RemoveApiScopeCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}