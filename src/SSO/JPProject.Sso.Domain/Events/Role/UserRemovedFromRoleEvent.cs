using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.Role
{
    public class UserRemovedFromRoleEvent : Event
    {
        public string Name { get; }
        public string Username { get; }

        public UserRemovedFromRoleEvent(string name, string username)
            : base(EventTypes.Success)
        {
            AggregateId = name;
            Name = name;
            Username = username;
        }
    }
}