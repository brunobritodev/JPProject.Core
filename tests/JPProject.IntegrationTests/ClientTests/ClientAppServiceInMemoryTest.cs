using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.ClientsViewModels;
using JPProject.Admin.Fakers.Test.ClientFakers;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Admin.IntegrationTests.ClientTests
{
    public class ClientAppServiceInMemoryTest : IClassFixture<WarmupInMemory>
    {
        private readonly IClientAppService _clientAppService;
        private readonly JPProjectAdminUIContext _database;

        public WarmupInMemory InMemoryData { get; }
        public ClientAppServiceInMemoryTest(WarmupInMemory inMemoryData)
        {
            InMemoryData = inMemoryData;
            _clientAppService = InMemoryData.Services.GetRequiredService<IClientAppService>();
            _database = InMemoryData.Services.GetRequiredService<JPProjectAdminUIContext>();
            var notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            notifications.Clear();
        }

        [Fact]
        public async Task ShouldAddNewClient()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Include(w => w.Client).Where(w => w.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldAddNewClientWithoutPostLogoutUri()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            _database.Clients.Include(w => w.PostLogoutRedirectUris).FirstOrDefault(w => w.ClientId == command.ClientId)
                .PostLogoutRedirectUris.Should().BeEmpty();
        }

        [Fact]
        public async Task ShouldRemoveClient()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Any().Should().BeTrue();

            await _clientAppService.Remove(new RemoveClientViewModel(command.ClientId));

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().BeNull();
        }

        [Fact]
        public async Task ShouldUpdateClient()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Any().Should().BeTrue();

            InMemoryData.DetachAll();

            var updateCommand = ClientFaker.GenerateClient().Generate();
            await _clientAppService.Update(command.ClientId, updateCommand);

            _database.Clients.FirstOrDefault(f => f.ClientId == updateCommand.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotAddAnotherClientWithSameClientId()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            var firstCall = await _clientAppService.Save(command);
            var result = await _clientAppService.Save(command);

            firstCall.Should().BeTrue();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldAddNewClientSecret()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var secret = ClientViewModelFaker.GenerateSaveClientSecret(command.ClientId);

            await _clientAppService.SaveSecret(secret);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientSecrets.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotAddNewClientSecretWhenClientDoesntExist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var secret = ClientViewModelFaker.GenerateSaveClientSecret(command.ClientId);

            var result = await _clientAppService.SaveSecret(secret);

            _database.ClientSecrets.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldAddNewClientProperty()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveProperty().Generate();

            await _clientAppService.SaveProperty(property);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientProperties.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotAddNewClientPropertyWhenClientDoesntExist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var property = ClientViewModelFaker.GenerateSaveProperty(command.ClientId).Generate();

            var result = await _clientAppService.SaveProperty(property);

            _database.ClientProperties.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldAddNewClientClaim()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveClaim().Generate();

            await _clientAppService.SaveClaim(property);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientClaims.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotAddNewClientClaimWhenClientDoesntExist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var property = ClientViewModelFaker.GenerateSaveClaim(command.ClientId).Generate();

            var result = await _clientAppService.SaveClaim(property);

            _database.ClientClaims.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
            result.Should().BeFalse();
        }
    }
}
