using JPProject.Domain.Core.Events;
using JPProject.Sso.Domain.Models;

namespace JPProject.Sso.Domain.Events.Email
{
    public class TemplateAddedEvent : Event
    {
        public Template Template { get; }

        public TemplateAddedEvent(Template template)
            : base(EventTypes.Success)
        {
            Template = template;
            AggregateId = template.Id.ToString();
            Message = "Template Added";
        }
    }
}
