using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.User
{
    public class EmailConfirmedEvent : Event
    {
        public string Email { get; }
        public string Code { get; }

        public EmailConfirmedEvent(string email, string code, string username)
            : base(EventTypes.Success)
        {
            Email = email;
            Code = code;
            AggregateId = username;
        }
    }
}
