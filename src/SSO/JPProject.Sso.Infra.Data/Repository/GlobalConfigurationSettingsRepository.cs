using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Infra.Data.Context;

namespace JPProject.Sso.Infra.Data.Repository
{
    public class GlobalConfigurationSettingsRepository : Repository<GlobalConfigurationSettings>, IGlobalConfigurationSettingsRepository
    {
        public GlobalConfigurationSettingsRepository(ApplicationSsoContext context) : base(context)
        {
        }
    }
}
