using FluentValidation;
using JPProject.Admin.Domain.Commands.ApiScope;

namespace JPProject.Admin.Domain.Validations.ApiResource
{
    public abstract class ApiScopeValidation<T> : AbstractValidator<T> where T : ApiScopeCommand
    {

        protected void ValidateScopeName()
        {
            RuleFor(c => c.ApiScope.Name).NotEmpty().WithMessage("Invalid ApiScope name");
        }

        protected void ValidateResourceName()
        {
            RuleFor(c => c.ResourceName).NotEmpty().WithMessage("Invalid name");
        }
        protected void ValidateOldName()
        {
            RuleFor(c => c.OldName).NotEmpty().WithMessage("Invalid Old Name");
        }
        protected void ValidateIfResourceExists()
        {
            RuleFor(c => c.ApiScope).Null().WithMessage("Could not have a ApiScope");
        }
    }
}