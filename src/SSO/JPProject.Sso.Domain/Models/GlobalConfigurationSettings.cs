using JPProject.Domain.Core.Models;

namespace JPProject.Sso.Domain.Models
{
    public class GlobalConfigurationSettings : Entity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Sensitive { get; set; }
        public bool Public { get; set; }

        public void Update(string value, in bool isPublic, in bool sensitive)
        {
            Value = value;
            Public = isPublic;
            Sensitive = sensitive;
        }
    }
}
