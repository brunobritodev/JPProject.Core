using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.Validations.GlobalConfiguration;

namespace JPProject.Sso.Domain.Commands.GlobalConfiguration
{
    public class ManageConfigurationCommand : GlobalConfigurationCommand
    {
        public ManageConfigurationCommand(string key, string value, bool sensitive, bool isPublic)
        {
            Key = key;
            Value = value;
            Sensitive = sensitive;
            IsPublic = isPublic;
        }
        public override bool IsValid()
        {
            ValidationResult = new CreateConfigurationValidation().Validate(this);
            return ValidationResult.IsValid;
        }

        public GlobalConfigurationSettings ToEntity()
        {
            return new GlobalConfigurationSettings(Key, Value, Sensitive, IsPublic);
        }
    }
}
