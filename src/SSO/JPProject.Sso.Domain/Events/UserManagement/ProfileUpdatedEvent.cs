using System;
using JPProject.Domain.Core.Events;
using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Events.UserManagement
{
    public class ProfileUpdatedEvent : Event
    {
        public UpdateProfileCommand Request { get; }

        public ProfileUpdatedEvent(Guid aggregateId, UpdateProfileCommand request)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId.ToString();
            Request = request;
        }
    }
}
