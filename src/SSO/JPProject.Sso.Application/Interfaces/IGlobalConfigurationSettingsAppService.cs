using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IGlobalConfigurationSettingsAppService : IGlobalConfigurationSettingsService
    {
        Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs);
    }
}