using System;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class UserRoleSavedEvent : Event
    {
        public string Username { get; }
        public string Role { get; }

        public UserRoleSavedEvent(Guid aggregateId, string username, string role)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId.ToString();
            Username = username;
            Role = role;
        }
    }
}