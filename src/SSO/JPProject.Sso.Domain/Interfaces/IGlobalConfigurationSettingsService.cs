using System.Threading.Tasks;
using JPProject.Sso.Domain.ViewModels.Settings;

namespace JPProject.Sso.Domain.Interfaces
{
    public interface IGlobalConfigurationSettingsService
    {
        Task<PrivateSettings> GetPrivateSettings();
        Task<PublicSettings> GetPublicSettings();
    }
}
