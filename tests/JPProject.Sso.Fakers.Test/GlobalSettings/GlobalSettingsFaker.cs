using Bogus;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.Settings;

namespace JPProject.Sso.Fakers.Test.GlobalSettings
{
    public class GlobalSettingsFaker
    {
        public static Faker<GlobalConfigurationSettings> GenerateSetting(string key = null, bool? sensitive = null, bool? isPublic = null)
        {
            return new Faker<GlobalConfigurationSettings>()
                .RuleFor(g => g.Key, f => key ?? f.PickRandom(Settings.AvailableSettings))
                .RuleFor(g => g.Value, f => f.Lorem.Word())
                .RuleFor(g => g.Sensitive, f => sensitive ?? f.Random.Bool())
                .RuleFor(g => g.Public, f => isPublic ?? f.Random.Bool())
                .RuleFor(g => g.Id, f => f.Random.Uuid());
        }

        public static Faker<ConfigurationViewModel> GenerateSettingViewModel(string key = null, bool? sensitive = null, bool? isPublic = null)
        {
            return new Faker<ConfigurationViewModel>()
                .RuleFor(g => g.Key, f => key ?? f.PickRandom(Settings.AvailableSettings))
                .RuleFor(g => g.Value, f => f.Lorem.Word())
                .RuleFor(g => g.IsSensitive, f => sensitive ?? f.Random.Bool())
                .RuleFor(g => g.IsPublic, f => isPublic ?? f.Random.Bool());
        }
    }
}
