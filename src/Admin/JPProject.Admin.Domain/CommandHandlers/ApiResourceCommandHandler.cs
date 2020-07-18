using JPProject.Admin.Domain.Commands.ApiResource;
using JPProject.Admin.Domain.Events.ApiResource;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.CommandHandlers
{
    public class ApiResourceCommandHandler : CommandHandler,
        IRequestHandler<RegisterApiResourceCommand, bool>,
        IRequestHandler<UpdateApiResourceCommand, bool>,
        IRequestHandler<RemoveApiResourceCommand, bool>,
        IRequestHandler<RemoveApiSecretCommand, bool>,
        IRequestHandler<SaveApiSecretCommand, bool>
    {
        private readonly IApiResourceRepository _apiRepository;

        public ApiResourceCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IApiResourceRepository apiRepository) : base(uow, bus, notifications)
        {
            _apiRepository = apiRepository;
        }


        public async Task<bool> Handle(RegisterApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.ApiResource.Name);
            if (savedClient != null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiResource", "ApiResource already exists"));
                return false;
            }

            _apiRepository.Add(request.ToModel());
            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiResourceRegisteredEvent(request.ApiResource));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(UpdateApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.OldResourceName);
            if (savedClient == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiResource", "ApiResource not found"));
                return false;
            }

            await _apiRepository.UpdateWithChildrens(request.OldResourceName, request.ApiResource);

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiResourceUpdatedEvent(request.ApiResource));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveApiResourceCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.ApiResource.Name);
            if (savedClient == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiResource", "ApiResource not found"));
                return false;
            }

            _apiRepository.Remove(savedClient);

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiResourceRemovedEvent(request.ApiResource.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveApiSecretCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiRepository.GetResource(request.ResourceName);
            if (savedClient == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiResource", "Api not found"));
                return false;
            }

            if (!savedClient.ApiSecrets.Any(f => f.Type == request.Type && f.Value == request.Value))
            {
                await Bus.RaiseEvent(new DomainNotification("Api Secret", "Invalid secret"));
                return false;
            }

            _apiRepository.RemoveSecret(request.ResourceName, request.ToModel());

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiSecretRemovedEvent(request.Type, request.ResourceName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveApiSecretCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedApi = await _apiRepository.GetResource(request.ResourceName);
            if (savedApi == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiResource", "Api not found"));
                return false;
            }


            _apiRepository.AddSecret(request.ResourceName, request.ToModel());

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiSecretSavedEvent(request.Type, request.ResourceName));
                return true;
            }
            return false;
        }

    }
}