using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Fakers.Test.GlobalSettings;
using JPProject.Sso.Infra.Data.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.GlobalSettings
{
    [Collection("GlobalSettings")]
    public class GlobalSettingsAdminTests : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationSsoContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private readonly IGlobalConfigurationSettingsAppService _globalSettingsAppService;
        private readonly AspNetUserTest _user;
        public WarmupInMemory InMemoryData { get; }

        public GlobalSettingsAdminTests(WarmupInMemory inMemory, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            InMemoryData = inMemory;
            _globalSettingsAppService = InMemoryData.Services.GetRequiredService<IGlobalConfigurationSettingsAppService>();
            _database = InMemoryData.Services.GetRequiredService<ApplicationSsoContext>();

            _user = (AspNetUserTest)InMemoryData.Services.GetService<ISystemUser>();
            _user._isInRole = true;
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }


        [Fact]
        public async Task ShouldNotTruncateSensitiveDataWhenUserIsAdmin()
        {
            var anotherSetting = GlobalSettingsFaker.GenerateSetting(key: "Smtp:Password", sensitive: true).Generate();
            _database.GlobalConfigurationSettings.Add(anotherSetting);
            await _database.SaveChangesAsync();

            var data = await _globalSettingsAppService.GetPrivateSettings();
            data.Smtp.Password.Should().NotContain("Sensitive Data");
        }
    }
}