using JPProject.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JPProject.Admin.Application.Interfaces
{
    public interface IIdentityServerEventStore
    {
        Task<IEnumerable<EventSelector>> ListAggregates();
    }
}