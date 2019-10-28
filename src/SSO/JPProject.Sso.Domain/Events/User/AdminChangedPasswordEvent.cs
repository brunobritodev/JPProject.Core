using System;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class AdminChangedPasswordEvent : Event
    {
        public string Username { get; }

        public AdminChangedPasswordEvent(Guid userId, string username)
            : base(EventTypes.Success)
        {
            Username = username;
            AggregateId = userId.ToString();
        }
    }
}