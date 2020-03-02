using System;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class PasswordChangedEvent : Event
    {

        public PasswordChangedEvent(string username)
            : base(EventTypes.Success)
        {
            AggregateId = username;
        }
    }
}