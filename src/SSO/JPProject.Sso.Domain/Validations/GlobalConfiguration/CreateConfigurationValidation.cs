using JPProject.Sso.Domain.Commands.GlobalConfiguration;

namespace JPProject.Sso.Domain.Validations.GlobalConfiguration
{
    public class CreateConfigurationValidation : GlobalConfigurationValidation<ManageConfigurationCommand>
    {
        public CreateConfigurationValidation()
        {
            ValidateValue();
            ValidateKey();
        }
    }
}
