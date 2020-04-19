using AutoMapper;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Commands.GlobalConfiguration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class GlobalConfigurationAppService : IGlobalConfigurationAppService
    {
        public IMediatorHandler Bus { get; }
        private readonly IMapper _mapper;
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;

        private readonly ISystemUser _systemUser;

        public GlobalConfigurationAppService(
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository,
            ISystemUser systemUser,
            IMediatorHandler bus)
        {
            Bus = bus;
            _mapper = GlobalConfigurationMapping.Mapper;
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;

            _systemUser = systemUser;
        }

        public async Task<PrivateSettings> GetPrivateSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.All();
            var privateSettings = new PrivateSettings(new Settings(settings));

            return privateSettings;
        }

        public async Task<PublicSettings> GetPublicSettings()
        {
            var settings = await _globalConfigurationSettingsRepository.All();

            var publicSettings = new PublicSettings(new Settings(settings));

            return publicSettings;
        }

        public async Task<bool> UpdateSettings(IEnumerable<ConfigurationViewModel> configs)
        {
            var success = true;
            foreach (var configurationViewModel in configs)
            {
                success = await Bus.SendCommand(_mapper.Map<ManageConfigurationCommand>(configurationViewModel));
                if (!success)
                    break;
            }
            return success;
        }

        public async Task<IEnumerable<ConfigurationViewModel>> ListSettings()
        {
            var settings = _mapper.Map<IEnumerable<ConfigurationViewModel>>(await _globalConfigurationSettingsRepository.All());
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
