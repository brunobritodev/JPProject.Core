using System;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class NewLoginAddedEvent : Event
    {
        public string Email { get; }
        public string Provider { get; }
        public string ProviderId { get; }

        public NewLoginAddedEvent(Guid aggregateId, string email, string provider, string providerId)
            : base(EventTypes.Success)
        {
            Email = email;
            Provider = provider;
            ProviderId = providerId;
            AggregateId = aggregateId.ToString();
        }
    }
}