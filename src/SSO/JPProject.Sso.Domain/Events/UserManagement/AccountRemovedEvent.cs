using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class AccountRemovedEvent : Event
    {
        public AccountRemovedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}