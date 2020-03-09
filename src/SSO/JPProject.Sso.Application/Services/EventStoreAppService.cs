using AspNetCore.IQueryable.Extensions;
using AspNetCore.IQueryable.Extensions.Filter;
using AutoMapper;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.EventSourcedNormalizers;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Services
{
    public class EventStoreAppService : IEventStoreAppService
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IGlobalConfigurationSettingsRepository _globalConfigurationSettingsRepository;
        private readonly IMapper _mapper;

        public EventStoreAppService(
            IEventStoreRepository eventStoreRepository,
            IGlobalConfigurationSettingsRepository globalConfigurationSettingsRepository
            )
        {
            _mapper = UserMapping.Mapper;
            _eventStoreRepository = eventStoreRepository;
            _globalConfigurationSettingsRepository = globalConfigurationSettingsRepository;
        }

        public ListOf<EventHistoryData> GetEvents(ICustomEventQueryable query)
        {
            var history = _eventStoreRepository.All().Apply((ICustomQueryable)query).ToList();
            var total = _eventStoreRepository.All().Filter(query).Count();
            if (total > 0)
                return new ListOf<EventHistoryData>(_mapper.Map<IEnumerable<EventHistoryData>>(history), total);

            return new ListOf<EventHistoryData>(new List<EventHistoryData>(), 0);
        }

        public async Task<IEnumerable<EventSelector>> ListAggregates()
        {
            var selector = new List<EventSelector>
            {
                new EventSelector(AggregateType.Email, EmailType.NewUser.ToString()),
                new EventSelector(AggregateType.Email, EmailType.NewUserWithoutPassword.ToString()),
                new EventSelector(AggregateType.Email, EmailType.RecoverPassword.ToString())
            };

            var keysGlobalConfig = await _globalConfigurationSettingsRepository.All();
            selector.AddRange(keysGlobalConfig.Select(s => new EventSelector(AggregateType.GlobalSettings, s.Key)));

            return selector;
        }
    }
}
