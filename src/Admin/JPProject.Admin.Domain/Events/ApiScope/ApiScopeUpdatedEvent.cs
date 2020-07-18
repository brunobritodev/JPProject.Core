using JPProject.Domain.Core.Events;

namespace JPProject.Admin.Domain.Events.ApiScope
{
    public class ApiScopeUpdatedEvent : Event
    {
        public IdentityServer4.Models.ApiScope ApiResource { get; }

        public ApiScopeUpdatedEvent(IdentityServer4.Models.ApiScope api)
        {
            ApiResource = api;
            AggregateId = api.Name;
        }
    }
}