using AutoMapper;
using IdentityServer4.Models;
using JPProject.Admin.Application.AutoMapper;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Domain.Commands.ApiScope;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using System;
using System.Threading.Tasks;
using JPProject.Admin.Domain.Interfaces;

namespace JPProject.Admin.Application.Services
{
    public class ApiScopeAppService : IApiScopeAppService
    {
        private IMapper _mapper;
        private IEventStoreRepository _eventStoreRepository;
        private readonly IApiScopeRepository _apiScopeRepository;
        public IMediatorHandler Bus { get; set; }

        public ApiScopeAppService(
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IApiScopeRepository apiScopeRepository)
        {
            _mapper = AdminApiResourceMapper.Mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _apiScopeRepository = apiScopeRepository;
        }

        public Task<bool> Remove(string name)
        {
            var registerCommand = new RemoveApiScopeCommand(name);
            return Bus.SendCommand(registerCommand);
        }


        public Task<bool> Save(ApiScope model)
        {
            var registerCommand = new SaveApiScopeCommand(model);
            return Bus.SendCommand(registerCommand);
        }

        public Task<bool> Update(string oldName, ApiScope apiScope)
        {
            var command = new UpdateApiScopeCommand(oldName, apiScope);
            return Bus.SendCommand(command);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
