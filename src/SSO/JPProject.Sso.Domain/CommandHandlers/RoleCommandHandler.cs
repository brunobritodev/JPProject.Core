using System.Threading;
using System.Threading.Tasks;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Commands;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.Commands.Role;
using JPProject.Sso.Domain.Events.Role;
using JPProject.Sso.Domain.Interfaces;
using MediatR;

namespace JPProject.Sso.Domain.CommandHandlers
{
    public class RoleCommandHandler : CommandHandler,
        IRequestHandler<RemoveRoleCommand, bool>,
        IRequestHandler<SaveRoleCommand, bool>,
        IRequestHandler<UpdateRoleCommand, bool>,
        IRequestHandler<RemoveUserFromRoleCommand, bool>
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RoleCommandHandler(
            IUnitOfWork uow,
            IMediatorHandler bus,
            INotificationHandler<DomainNotification> notifications,
            IRoleService roleService,
            IUserService userService) : base(uow, bus, notifications)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<bool> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Remove(request.Name);

            if (result)
            {
                await Bus.RaiseEvent(new RoleRemovedEvent(request.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(SaveRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Save(request.Name);

            if (result)
            {
                await Bus.RaiseEvent(new RoleSavedEvent(request.Name));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _roleService.Update(request.Name, request.OldName);

            if (result)
            {
                await Bus.RaiseEvent(new RoleUpdatedEvent(request.Name, request.OldName));
                return true;
            }
            return false;
        }

        public async Task<bool> Handle(RemoveUserFromRoleCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);
                return false;
            }

            // Businness logic here
            var result = await _userService.RemoveUserFromRole(request.Name, request.Username);

            if (result)
            {
                await Bus.RaiseEvent(new UserRemovedFromRoleEvent(request.Name, request.Username));
                return true;
            }
            return false;
        }
    }
}
