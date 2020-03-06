using JPProject.Domain.Core.Events;
using JPProject.Domain.Core.Util;

namespace JPProject.Sso.Application.EventSourcedNormalizers
{
    public class EventHistoryData
    {
        public EventHistoryData(string action, string aggregate, EventDetails details, string when, string who,
            string category, string ip, EventTypes eventType)
        {
            Action = action;
            When = when;
            Who = who;
            Aggregate = aggregate;
            Category = category;
            Ip = ip;
            if ((int)eventType > 0)
                EventType = eventType.ToString().AddSpacesToSentence();
            Details = details?.Metadata;
        }

        public string Category { get; }
        public string Ip { get; }
        public string EventType { get; }
        public string Action { get; }
        public string Aggregate { get; private set; }
        public string When { get; }
        public string Who { get; private set; }

        public string Details { get; private set; }

        public void MarkAsSensitiveData()
        {
            if (!Who.IsNullOrEmpty() && Who.Contains('@'))
                Who = Who.TruncateEmail();
            else
                Who = Who?.TruncateSensitiveInformation();

            Details = Details?.TruncateSensitiveInformation();
            Aggregate = Aggregate?.TruncateSensitiveInformation();
        }
    }
}