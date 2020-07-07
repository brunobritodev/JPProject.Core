using AutoMapper;
using JPProject.Admin.Application.AutoMapper;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.ApiScopeViewModels;
using JPProject.Admin.Domain.Commands.ApiScope;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using System;
using System.Threading.Tasks;

namespace JPProject.Admin.Application.Services
{
    public class ApiScopeAppService : IApiScopeAppService
    {
        private IMapper _mapper;
        private IEventStoreRepository _eventStoreRepository;
        private readonly IApiScopeAppService _apiScopeAppService;
        public IMediatorHandler Bus { get; set; }

        public ApiScopeAppService(
            IMediatorHandler bus,
            IEventStoreRepository eventStoreRepository,
            IApiScopeAppService apiScopeAppService)
        {
            _mapper = AdminApiResourceMapper.Mapper;
            Bus = bus;
            _eventStoreRepository = eventStoreRepository;
            _apiScopeAppService = apiScopeAppService;
        }

        public Task<bool> RemoveScope(RemoveApiScopeViewModel model)
        {
            var registerCommand = _mapper.Map<RemoveApiScopeCommand>(model);
            return Bus.SendCommand(registerCommand);
        }


        public Task<bool> SaveScope(SaveApiScopeViewModel model)
        {
            var registerCommand = _mapper.Map<SaveApiScopeCommand>(model);
            return Bus.SendCommand(registerCommand);
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
