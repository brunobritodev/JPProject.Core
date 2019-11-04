using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class AccountRemovedEvent : Event
    {
        public AccountRemovedEvent(string aggregateId)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId.ToString();
        }
    }
}