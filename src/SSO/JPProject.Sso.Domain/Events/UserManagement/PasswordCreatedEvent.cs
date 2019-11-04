using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class PasswordCreatedEvent : Event
    {

        public PasswordCreatedEvent(string aggregateId)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId;
        }
    }
}