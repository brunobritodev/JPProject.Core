using System.Collections.Generic;
using System.Security.Claims;
using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class ClaimsSyncronizedEvent : Event
    {
        public IEnumerable<Claim> Claims { get; }

        public ClaimsSyncronizedEvent(string username, IEnumerable<Claim> claims)
            : base(EventTypes.Success)
        {
            Claims = claims;
            AggregateId = username;
        }
    }
}