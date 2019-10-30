using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.IdentityResourceViewModels;
using JPProject.Admin.Fakers.Test.IdentityResourceFakers;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace JPProject.Admin.IntegrationTests.IdentityResourceTests
{
    public class IdentityResourceAppServiceTests : IClassFixture<WarmupInMemory>
    {
        private readonly IIdentityResourceAppService _identityResource;
        private readonly JPProjectAdminUIContext _database;
        public WarmupInMemory InMemoryData { get; }

        public IdentityResourceAppServiceTests(WarmupInMemory inMemoryData)
        {
            InMemoryData = inMemoryData;
            _identityResource = InMemoryData.Services.GetRequiredService<IIdentityResourceAppService>();
            _database = InMemoryData.Services.GetRequiredService<JPProjectAdminUIContext>();
            var notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            notifications.Clear();
        }


        [Fact]
        public async Task ShouldAddNewIdentityResource()
        {
            var command = IdentityResourceFaker.GenerateIdentiyResource().Generate();

            await _identityResource.Save(command);

            _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldRemoveIdentityResource()
        {
            var command = IdentityResourceFaker.GenerateIdentiyResource().Generate();

            await _identityResource.Save(command);

            _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name).Should().NotBeNull();

            await _identityResource.Remove(new RemoveIdentityResourceViewModel(command.Name));

            _database.IdentityResources.FirstOrDefault(s => s.Name == command.Name).Should().BeNull();
        }

        [Fact]
        public async Task ShouldUpdateIdentityResource()
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
