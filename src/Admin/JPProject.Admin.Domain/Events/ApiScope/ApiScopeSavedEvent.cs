using JPProject.Domain.Core.Events;

namespace JPProject.Admin.Domain.Events.ApiScope
{
    public class ApiScopeSavedEvent : Event
    {
        public IdentityServer4.Models.ApiScope Scope { get; }

        public ApiScopeSavedEvent(IdentityServer4.Models.ApiScope scope)
        {
            Scope = scope;
            AggregateId = scope.Name;
        }
    }
}