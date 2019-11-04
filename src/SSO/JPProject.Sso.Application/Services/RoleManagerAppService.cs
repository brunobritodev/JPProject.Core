using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels.RoleViewModels;
using JPProject.Sso.Domain.Commands.Role;
using JPProject.Sso.Domain.Interfaces;

namespace JPProject.Sso.Application.Services
{
    public class RoleManagerAppService : IRoleManagerAppService
    {
        private IEventStoreRepository _eventStoreRepository;
        private IMapper _mapper;
        private readonly IRoleService _roleService;

        public IMediatorHandler Bus { get; set; }
        public RoleManagerAppService(IMapper mapper,
            IRoleService roleService,
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository
        )
        {
            _mapper = mapper;
            _roleService = roleService;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRoles()
        {
            return _mapper.Map<IEnumerable<RoleViewModel>>(await _roleService.GetAllRoles());
        }

        public Task Remove(RemoveRoleViewModel model)
        {
            var command = _mapper.Map<RemoveRoleCommand>(model);
            return Bus.SendCommand(command);
        }

        public async Task<RoleViewModel> GetDetails(string name)
        {
            return _mapper.Map<RoleViewModel>(await _roleService.Details(name));
        }

        public Task Save(SaveRoleViewModel model)
        {
            var command = _mapper.Map<SaveRoleCommand>(model);
            return Bus.SendCommand(command);
        }

        public Task Update(string id, UpdateRoleViewModel model)
        {
            var command = new UpdateRoleCommand(model.Name, id);
            return Bus.SendCommand(command);
        }

        public Task RemoveUserFromRole(RemoveUserFromRoleViewModel model)
        {
            var command = _mapper.Map<RemoveUserFromRoleCommand>(model);
            return Bus.SendCommand(command);
        }
    }
}