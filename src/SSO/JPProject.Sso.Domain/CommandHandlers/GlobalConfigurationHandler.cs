using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.Commands.GlobalConfiguration;
using JPProject.Sso.Domain.Events.GlobalConfiguration;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JPProject.Sso.Domain.CommandHandlers
{
    public class GlobalConfigurationHandler :
        CommandHandler,
        IRequestHandler<ManageConfigurationCommand, bool>
    {
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;

        public GlobalConfigurationHandler(
            ISsoUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository) : base(uow, bus, notifications)
        {
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
        }


        public async Task<bool> Handle(ManageConfigurationCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var setting = await _globalConfigurationSettingsRepository.FindByKey(request.Key);
            if (setting is null)
                return await CreateConfiguration(request);

            return await UpdateConfiguration(setting, request);
        }

        public async Task<bool> UpdateConfiguration(
            GlobalConfigurationSettings setting,
            ManageConfigurationCommand request)
        {
            setting.Update(request.Value, request.IsPublic, request.Sensitive);
            _globalConfigurationSettingsRepository.Update(setting);

            if (await Commit())
            {
                await Bus.RaiseEvent(new GlobalConfigurationUpdatedEvent(request.Key, request.Sensitive ? "Sensitive information" : request.Value, request.IsPublic, request.Sensitive));
                return true;
            }

            return false;
        }

        private async Task<bool> CreateConfiguration(ManageConfigurationCommand request)
        {
            // Businness logic here
            _globalConfigurationSettingsRepository.Add(request.ToEntity());

            if (await Commit())
            {
                await Bus.RaiseEvent(new GlobalConfigurationCreatedEvent(request.Key,
                    request.Sensitive ? "Sensitive information" : request.Value, request.IsPublic, request.Sensitive));
                return true;
            }

            return false;
        }
    }
}
