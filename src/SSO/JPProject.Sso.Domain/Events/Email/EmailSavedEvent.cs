using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.Email
{
    public class EmailSavedEvent : Event
    {
        public Models.Email Email { get; }

        public EmailSavedEvent(Models.Email email)
            : base(EventTypes.Success)
        {
            Email = email;
            AggregateId = email.Type.ToString();
        }

    }
}