using JPProject.Domain.Core.Events;
using System;

namespace JPProject.Sso.Domain.Events.Email
{
    public class TemplateRemovedEvent : Event
    {
        public TemplateRemovedEvent(Guid templateId)
        {
            AggregateId = templateId.ToString();
            Message = "Template";
        }
    }
}