using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;

namespace JPProject.Sso.Application.Services
{
    public class GlobalConfigurationSettingsService : IGlobalConfigurationSettingsAppService
    {
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;
        private readonly ISystemUser _systemUser;
        private readonly IUnitOfWork _unitOfWork;

        public GlobalConfigurationSettingsService(
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository,
            ISystemUser systemUser,
            IUnitOfWork unitOfWork)
        {
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
            _systemUser = systemUser;
            _unitOfWork = unitOfWork;
        }

        public async Task<PrivateSettings> GetPrivateSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.GetAll().ToListAsync();
            if (!_systemUser.IsInRole("Administrator"))
            {
                foreach (var globalConfigurationSettingse in settings.Where(globalConfigurationSettingse => globalConfigurationSettingse.Sensitive))
                {
                    globalConfigurationSettingse.Value = "**Sensitive Data**";
                }
            }
            var privateSettings = new PrivateSettings(new Settings(settings));

            return privateSettings;
        }

        public async Task<PublicSettings> GetPublicSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.GetAll().ToListAsync();

            var publicSettings = new PublicSettings(new Settings(settings.Where(w => w.Public && !w.Sensitive)));

            return publicSettings;
        }

        public async Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs)
        {
            var settings = await _globalConfigurationSettingsRepository.GetAll().ToListAsync();
            foreach (var configurationViewModel in configs)
            {
                var setting = settings.FirstOrDefault(f => f.Key.Equals(configurationViewModel.Key));
                if (setting is null)
                    continue;

                setting.Update(configurationViewModel.Value, configurationViewModel.IsPublic, configurationViewModel.IsSensitve);
                _globalConfigurationSettingsRepository.Update(setting);
            }

            await _unitOfWork.Commit();
            return true;
        }
    }
}
