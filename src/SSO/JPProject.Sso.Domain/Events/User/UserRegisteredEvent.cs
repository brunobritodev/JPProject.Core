using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class UserRegisteredEvent : Event
    {
        public string Username { get; }
        public string UserEmail { get; }

        public UserRegisteredEvent(string aggregateId, string userName, string userEmail)
            : base(EventTypes.Success)
        {
            AggregateId = aggregateId.ToString();
            Username = userName;
            UserEmail = userEmail;
        }
    }
}