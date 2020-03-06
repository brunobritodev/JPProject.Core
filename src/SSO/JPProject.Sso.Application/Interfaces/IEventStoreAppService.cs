using JPProject.Domain.Core.ViewModels;
using JPProject.Sso.Application.EventSourcedNormalizers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.Interfaces
{
    public interface IEventStoreAppService
    {
        ListOf<EventHistoryData> GetEvents(ICustomEventQueryable query);
        Task<IEnumerable<EventSelector>> ListAggregates();
    }
}