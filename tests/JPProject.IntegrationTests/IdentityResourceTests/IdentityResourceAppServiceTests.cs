using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Admin.Fakers.Test.IdentityResourceFakers;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Admin.IntegrationTests.IdentityResourceTests
{
    public class IdentityResourceAppServiceTests : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly IIdentityResourceAppService _identityResource;
        private readonly JPProjectAdminUIContext _database;
        private DomainNotificationHandler _notifications;
        public WarmupInMemory InMemoryData { get; }

        public IdentityResourceAppServiceTests(WarmupInMemory inMemoryData, ITestOutputHelper output)
        {
            _output = output;
            InMemoryData = inMemoryData;
            _identityResource = InMemoryData.Services.GetRequiredService<IIdentityResourceAppService>();
            _database = InMemoryData.Services.GetRequiredService<JPProjectAdminUIContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            _notifications.Clear();
        }


        [Fact]
        public async Task Should_Add_New_IdentityResource()
        {
            var command = IdentityResourceFaker.GenerateIdentiyResource().Generate();

            await _identityResource.Save(command);

            var idrs = _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name);
            idrs.Should().NotBeNull();

        }

        [Fact]
        public async Task Should_Remove_IdentityResource()
        {
            var command = IdentityResourceFaker.GenerateIdentiyResource().Generate();

            await _identityResource.Save(command);

            _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name).Should().NotBeNull();

            await _identityResource.Remove(new RemoveIdentityResourceViewModel(command.Name));
            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);
            _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name).Should().BeNull();
        }

        [Fact]
        public async Task Should_Update_IdentityResource()
        {
            var command = IdentityResourceFaker.GenerateIdentiyResource().Generate();

            await _identityResource.Save(command);

            var client = _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name);
            client.Should().NotBeNull();

            InMemoryData.DetachAll();

            var updateCommand = IdentityResourceFaker.GenerateIdentiyResource().Generate();
            await _identityResource.Update(command.Name, updateCommand);

            _database.IdentityResources.FirstOrDefault(f => f.Name == updateCommand.Name).Should().NotBeNull();
        }
    }
}
