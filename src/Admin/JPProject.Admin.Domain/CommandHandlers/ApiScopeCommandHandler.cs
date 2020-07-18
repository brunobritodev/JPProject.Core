using JPProject.Admin.Domain.Commands.ApiScope;
using JPProject.Admin.Domain.Events.ApiScope;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace JPProject.Admin.Domain.CommandHandlers
{
    public class ApiScopeCommandHandler : CommandHandler,
        IRequestHandler<RemoveApiScopeCommand, bool>,
        IRequestHandler<SaveApiScopeCommand, bool>,
        IRequestHandler<UpdateApiScopeCommand, bool>
    {
        private readonly IApiScopeRepository _apiScopeRepository;

        public ApiScopeCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IApiScopeRepository apiScopeRepository) : base(uow, bus, notifications)
        {
            _apiScopeRepository = apiScopeRepository;
        }

        public async Task<bool> Handle(RemoveApiScopeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiScopeRepository.Get(request.ResourceName);
            if (savedClient == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiScope", "ApiScope not found"));
                return false;
            }

            _apiScopeRepository.RemoveScope(request.ResourceName);

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiScopeRemovedEvent(request.ResourceName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveApiScopeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedApi = await _apiScopeRepository.Get(request.ApiScope.Name);
            if (savedApi != null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiScope", "ApiScope already exists"));
                return false;
            }

            var scope = request.ToModel();
            _apiScopeRepository.Add(scope);

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiScopeSavedEvent(scope));
                return true;
            }
            return false;
        }


        public async Task<bool> Handle(UpdateApiScopeCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            var savedClient = await _apiScopeRepository.Get(request.OldName);
            if (savedClient == null)
            {
                await Bus.RaiseEvent(new DomainNotification("ApiScope", "ApiScope not found"));
                return false;
            }

            var scope = request.ToModel();
            await _apiScopeRepository.UpdateWithChildren(request.OldName, scope);

            if (await Commit())
            {
                await Bus.RaiseEvent(new ApiScopeUpdatedEvent(scope));
                return true;
            }
            return false;
        }
    }
}