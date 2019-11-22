using JPProject.Sso.Domain.Models;
using System.Collections.Generic;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class Settings : Dictionary<string, string>
    {
        public new string this[string key] => ContainsKey(key) ? base[key] : null;

        public static readonly string[] AvailableSettings = { "Smtp:Server", "Smtp:Password", "Smtp:Username", "Smtp:Port", "Smtp:UseSsl", "SendEmail" };

        public Settings(IEnumerable<GlobalConfigurationSettings> configuration)
        {
            foreach (var globalConfigurationSettingse in configuration)
            {
                TryAdd(globalConfigurationSettingse.Key, globalConfigurationSettingse.Value);
            }
        }
    }
}
