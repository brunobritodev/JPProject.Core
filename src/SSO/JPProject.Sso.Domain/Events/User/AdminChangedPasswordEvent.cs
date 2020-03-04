using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class AdminChangedPasswordEvent : Event
    {
        public AdminChangedPasswordEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}