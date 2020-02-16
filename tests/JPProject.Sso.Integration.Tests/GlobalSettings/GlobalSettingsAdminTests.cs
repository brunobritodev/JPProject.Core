using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Fakers.Test.GlobalSettings;
using JPProject.Sso.Infra.Data.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.GlobalSettings
{
    [Collection("GlobalSettings")]
    public class GlobalSettingsTests : IClassFixture<WarmupUnifiedContext>
    {
        private readonly ITestOutputHelper _output;
        private readonly ISsoContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private readonly IGlobalConfigurationAppService _globalAppService;
        private readonly AspNetUserTest _user;
        public WarmupUnifiedContext UnifiedContextData { get; }

        public GlobalSettingsTests(WarmupUnifiedContext unifiedContext, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            UnifiedContextData = unifiedContext;
            _globalAppService = UnifiedContextData.Services.GetRequiredService<IGlobalConfigurationAppService>();
            _database = UnifiedContextData.Services.GetRequiredService<ISsoContext>();

            _user = (AspNetUserTest)UnifiedContextData.Services.GetService<ISystemUser>();
            _user._isInRole = true;
            _notifications = (DomainNotificationHandler)UnifiedContextData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }


        [Fact]
        public async Task Should_Not_Truncate_Sensitive_Data_When_User_Is_Admin()
        {
            var anotherSetting = GlobalSettingsFaker.GenerateSetting(key: "Smtp:Password", sensitive: true).Generate();
            _database.GlobalConfigurationSettings.Add(anotherSetting);
            await _database.SaveChangesAsync();

            var data = await _globalAppService.GetPrivateSettings();
            data.Smtp.Password.Should().NotContain("Sensitive Data");
        }


        [Fact]
        public async Task Should_Update_When_Configuration_Exist()
        {
            var setting = GlobalSettingsFaker.GenerateSetting(key: "Smtp:Password").Generate();
            var oldSetting = setting.Value;
            _database.GlobalConfigurationSettings.Add(setting);
            await _database.SaveChangesAsync();

            var anotherSetting = GlobalSettingsFaker.GenerateSettingViewModel(key: "Smtp:Password").Generate();

            await _globalAppService.UpdateSettings(new List<ConfigurationViewModel>() { anotherSetting });
            var data = await _globalAppService.GetPrivateSettings();
            data.Smtp.Password.Should().NotBeEquivalentTo(oldSetting);
        }

        [Fact]
        public async Task Should_Save_Configuration_When_It_Doesnt_Exist()
        {
            var anotherSetting = GlobalSettingsFaker.GenerateSettingViewModel(key: "Smtp:Password", isPublic: false).Generate();

            await _globalAppService.UpdateSettings(new List<ConfigurationViewModel>() { anotherSetting });
            var data = await _globalAppService.GetPrivateSettings();
            data.Smtp.Password.Should().NotBeEmpty();
        }

    }
}