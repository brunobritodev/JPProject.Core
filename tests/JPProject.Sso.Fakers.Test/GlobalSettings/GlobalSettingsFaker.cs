using Bogus;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.Settings;

namespace JPProject.Sso.Fakers.Test.GlobalSettings
{
    public class GlobalSettingsFaker
    {
        public static Faker<GlobalConfigurationSettings> GenerateSetting(string key = null, bool? sensitive = null)
        {
            return new Faker<GlobalConfigurationSettings>()
                .RuleFor(g => g.Key, f => key ?? f.PickRandom(Settings.AvailableSettings))
                .RuleFor(g => g.Value, f => f.Lorem.Word())
                .RuleFor(g => g.Sensitive, f => sensitive ?? f.Random.Bool())
                .RuleFor(g => g.Public, f => f.Random.Bool())
                .RuleFor(g => g.Id, f => f.Random.Uuid());
        }
    }
}
