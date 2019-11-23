using JPProject.Domain.Core.Models;
using System;

namespace JPProject.Sso.Domain.Models
{
    public class GlobalConfigurationSettings : Entity
    {
        public GlobalConfigurationSettings()
        {
            Id = Guid.NewGuid();
        }
        public GlobalConfigurationSettings(string key, string value, bool sensitive, bool isPublic)
        {
            Key = key;
            Value = value;
            Sensitive = sensitive;
            IsPublic = isPublic;
            Id = Guid.NewGuid();
        }
        public string Key { get; private set; }
        public string Value { get; private set; }
        public bool Sensitive { get; private set; }
        public bool IsPublic { get; }
        public bool Public { get; private set; }

        public void Update(string value, in bool isPublic, in bool sensitive)
        {
            Value = value;
            Public = isPublic;
            Sensitive = sensitive;
        }

        public void UpdateSensitive()
        {
            if (Sensitive)
                Value = "**Sensitive Data**";
        }
    }
}
