using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class PasswordCreatedEvent : Event
    {

        public PasswordCreatedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}