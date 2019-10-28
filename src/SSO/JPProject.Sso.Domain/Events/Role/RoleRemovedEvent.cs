using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.Role
{
    public class RoleRemovedEvent : Event
    {
        public RoleRemovedEvent(string name)
            : base(EventTypes.Success)
        {
            AggregateId = name;
        }
    }
}
