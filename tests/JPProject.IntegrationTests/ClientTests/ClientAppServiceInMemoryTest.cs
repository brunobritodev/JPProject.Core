using Bogus;
using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.ClientsViewModels;
using JPProject.Admin.Domain.Commands.Clients;
using JPProject.Admin.Fakers.Test.ClientFakers;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Admin.IntegrationTests.ClientTests
{
    public class ClientAppServiceInMemoryTest : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly IClientAppService _clientAppService;
        private readonly JPProjectAdminUIContext _database;
        private readonly DomainNotificationHandler _notifications;
        private Faker _faker;

        public WarmupInMemory InMemoryData { get; }
        public ClientAppServiceInMemoryTest(WarmupInMemory inMemoryData, ITestOutputHelper output)
        {
            _faker = new Faker();
            _output = output;
            InMemoryData = inMemoryData;
            _clientAppService = InMemoryData.Services.GetRequiredService<IClientAppService>();
            _database = InMemoryData.Services.GetRequiredService<JPProjectAdminUIContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            _notifications.Clear();
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
        public async Task ShouldAddNewClientWithoutPostLogout()
        {
            //var command = JsonConvert.DeserializeObject<SaveClientViewModel>("{\"ClientId\":\"aoeu123\",\"ClientName\":\"aoe\",\"LogoUri\":\"https://localhost:5000/storage/1200px-Jenkins_logo.svg.png\",\"ClientType\":\"WebImplicit\"}");
            var command = JsonConvert.DeserializeObject<SaveClientViewModel>("{\"ClientId\":\"aoeu123\",\"ClientName\":\"aoe\",\"ClientType\":\"WebImplicit\"}");

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Include(w => w.Client).Where(w => w.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Theory]
        [InlineData(ClientType.Spa, new[] { "openid", "profile" })]
        [InlineData(ClientType.WebHybrid, new[] { "openid", "profile" })]
        [InlineData(ClientType.WebImplicit, new[] { "openid", "profile" })]
        [InlineData(ClientType.Device, new[] { "openid" })]
        [InlineData(ClientType.Machine, new[] { "openid" })]
        [InlineData(ClientType.Native, new[] { "openid", "profile" })]
        public async Task ShouldAddDefaultScopes(ClientType clientType, string[] scopeName)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();

            foreach (var s in scopeName)
            {
                client?.AllowedScopes.Should().Contain(scope => scope.Scope.Equals(s));
            }
        }

        [Theory]
        [InlineData(ClientType.Spa)]
        [InlineData(ClientType.WebHybrid)]
        [InlineData(ClientType.WebImplicit)]
        public async Task ShouldAddCors(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.AllowedCorsOrigins.Should().Contain(origin => origin.Origin.Equals(command.ClientUri));
        }

        [Theory]
        [InlineData(ClientType.Spa)]
        [InlineData(ClientType.WebHybrid)]
        [InlineData(ClientType.WebImplicit)]
        public async Task ShouldAddDefaultRedirectUri(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.RedirectUris.Should().Contain(origin => origin.RedirectUri.Equals(command.ClientUri));
        }


        [Fact]
        public async Task ShouldAddClientServer()
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: ClientType.Machine).Generate();
            command.ClientUri = null;
            command.LogoUri = null;
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.RedirectUris.Should().BeEmpty();
            client.RequireConsent.Should().BeFalse();
        }



        [Theory]
        [InlineData(ClientType.Spa)]
        [InlineData(ClientType.WebHybrid)]
        [InlineData(ClientType.WebImplicit)]
        public async Task ShouldAddDefaultLogoutUriIfNull(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.PostLogoutRedirectUris.Should().Contain(origin => origin.PostLogoutRedirectUri.Equals(command.ClientUri));
        }


        [Theory]
        [InlineData(ClientType.Spa)]
        [InlineData(ClientType.WebImplicit)]
        public async Task ShouldAddAlwaysIncludeUserClaimsInIdTokenWhenImplicity(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.AlwaysIncludeUserClaimsInIdToken.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldNotSaveClientWithPostLogoutUriWithTrailingSlash()
        {
            var command = ClientViewModelFaker.GenerateSaveClient(logoutUri: $"{_faker.Internet.Url()}/").Generate();
            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().BeNull();
        }


        [Fact]
        public async Task ShouldNotSaveClientWithClientUriWithTrailingSlash()
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientUri: $"{_faker.Internet.Url()}/").Generate();
            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().BeNull();
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

            firstCall.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
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
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
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

            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);

            _database.ClientProperties.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();

            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
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
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
        }
    }
}
