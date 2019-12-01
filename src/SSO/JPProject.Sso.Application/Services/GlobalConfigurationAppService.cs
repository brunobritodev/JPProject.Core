using AutoMapper;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels.Settings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class GlobalConfigurationAppService : IGlobalConfigurationAppService
    {
        private readonly IMapper _mapper;
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemUser _systemUser;

        public GlobalConfigurationAppService(
            IMapper mapper,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository,
            ISsoUnitOfWork unitOfWork,
            ISystemUser systemUser)
        {
            _mapper = mapper;
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
            _unitOfWork = unitOfWork;
            _systemUser = systemUser;
        }

        public async Task<PrivateSettings> GetPrivateSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.GetAll().ToListAsync();
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

                setting.Update(configurationViewModel.Value, configurationViewModel.IsPublic, configurationViewModel.IsSensitive);
                _globalConfigurationSettingsRepository.Update(setting);
            }

            await _unitOfWork.Commit();
            return true;
        }

        public async Task<IEnumerable<ConfigurationViewModel>> ListSettings()
        {
            var settings = _mapper.Map<IEnumerable<ConfigurationViewModel>>(await _globalConfigurationSettingsRepository.GetAll().ToListAsync());
            if (!_systemUser.IsInRole("Administrator"))
            {
                foreach (var configurationViewModel in settings)
                {
                    configurationViewModel.UpdateSensitive();
                }
            }
            return settings;
        }
    }
}
