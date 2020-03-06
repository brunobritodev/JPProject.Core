using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Application.Services
{
    public class IdentityServerEventStoreAppService : IIdentityServerEventStore
    {
        private readonly IClientRepository _clientRepository;
        private readonly IIdentityResourceRepository _identityResourceRepository;
        private readonly IApiResourceRepository _apiResourceRepository;

        public IdentityServerEventStoreAppService(
            IClientRepository clientRepository,
            IIdentityResourceRepository identityResourceRepository,
            IApiResourceRepository apiResourceRepository)
        {
            _clientRepository = clientRepository;
            _identityResourceRepository = identityResourceRepository;
            _apiResourceRepository = apiResourceRepository;
        }

        public async Task<IEnumerable<EventSelector>> ListAggregates()
        {
            var eventsSelector = new List<EventSelector>();
            var clients = await _clientRepository.ListClients();
            var identityResources = await _identityResourceRepository.ListIdentityResources();
            var apiResources = await _apiResourceRepository.ListResources();

            eventsSelector.AddRange(clients.Select(s => new EventSelector(AggregateType.Client, s)));
            eventsSelector.AddRange(identityResources.Select(s => new EventSelector(AggregateType.IdentityResource, s)));
            eventsSelector.AddRange(apiResources.Select(s => new EventSelector(AggregateType.ApiResource, s)));

            return eventsSelector;
        }
    }
}
