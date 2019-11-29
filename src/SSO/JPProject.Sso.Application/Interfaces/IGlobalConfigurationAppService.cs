using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IGlobalConfigurationAppService : IGlobalConfigurationSettingsService
    {
        Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs);
        Task<IEnumerable<ConfigurationViewModel>> ListSettings();
    }
}