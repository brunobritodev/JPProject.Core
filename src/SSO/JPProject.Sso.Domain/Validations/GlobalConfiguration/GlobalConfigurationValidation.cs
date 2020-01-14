using FluentValidation;
using JPProject.Sso.Domain.Commands.GlobalConfiguration;

namespace JPProject.Sso.Domain.Validations.GlobalConfiguration
{
    public abstract class GlobalConfigurationValidation<T> : AbstractValidator<T> where T : GlobalConfigurationCommand
    {
        protected void ValidateKey()
        {
            RuleFor(r => r.Key).NotEmpty();
        }

        protected void ValidateValue()
        {
            RuleFor(r => r.Key).NotEmpty();
        }
    }
}
