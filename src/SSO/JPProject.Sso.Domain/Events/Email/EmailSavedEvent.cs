using JPProject.Domain.Core.Events;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Domain.Events.Email
{
    public class EmailSavedEvent : Event
    {

        public EmailSavedEvent(Models.Email email)
            : base(EventTypes.Success)
        {
            AggregateId = email.Id.ToString();
            Type = email.Type;
            Message = "Email";
        }

        public EmailType Type { get; set; }
    }
}