using System;
using JPProject.Domain.Core.Events;
using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class ProfileUpdatedEvent : Event
    {
        public UpdateProfileCommand Request { get; }

        public ProfileUpdatedEvent(string username, UpdateProfileCommand request)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Request = request;
        }
    }
}
