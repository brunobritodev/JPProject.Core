using System;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class UserRoleSavedEvent : Event
    {
        public string Username { get; }
        public string Role { get; }

        public UserRoleSavedEvent(string aggregateId, string username, string role)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId;
            Username = username;
            Role = role;
        }
    }
}