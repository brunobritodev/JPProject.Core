using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class AccountPasswordResetedEvent : Event
    {
        public string Email { get; }
        public string Code { get; }

        public AccountPasswordResetedEvent(string username, string email, string code)
            : base(EventTypes.Success)
        {
            AggregateId = username;
            Email = email;
            Code = code;
        }
    }
}