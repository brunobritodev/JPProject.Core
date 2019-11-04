using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.Role
{
    public class RoleSavedEvent : Event
    {
        public RoleSavedEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}