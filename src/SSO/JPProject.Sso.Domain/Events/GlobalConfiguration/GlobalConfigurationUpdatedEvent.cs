using JPProject.Domain.Core.Events;

namespace JPProject.Sso.Domain.Events.GlobalConfiguration
{
    public class GlobalConfigurationUpdatedEvent : Event
    {
        public string Key { get; }
        public string Value { get; }
        public bool IsPublic { get; }
        public bool Sensitive { get; }

        public GlobalConfigurationUpdatedEvent(string key, string value, in bool isPublic, in bool sensitive)
        {
            Key = key;
            Value = value;
            IsPublic = isPublic;
            Sensitive = sensitive;
        }
    }
}