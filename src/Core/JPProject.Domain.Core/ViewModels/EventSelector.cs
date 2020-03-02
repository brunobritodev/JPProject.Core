using JPProject.Domain.Core.Util;

namespace JPProject.Domain.Core.ViewModels
{
    public class EventSelector
    {
        public EventSelector() { }
        public EventSelector(AggregateType type, string aggregate)
        {
            AggregateType = type.ToString().AddSpacesToSentence();
            Aggregate = aggregate;
        }

        public string AggregateType { get; set; }
        public string Aggregate { get; set; }
    }
}
