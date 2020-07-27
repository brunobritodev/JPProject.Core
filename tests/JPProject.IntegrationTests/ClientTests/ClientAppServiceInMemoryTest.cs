using Bogus;
using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.ClientsViewModels;
using JPProject.Admin.Domain.Commands.Clients;
using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.Admin.Fakers.Test.ClientFakers;
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
        private readonly JpProjectAdminUiContext _database;
        private readonly DomainNotificationHandler _notifications;
        private Faker _faker;

        public WarmupInMemory InMemoryData { get; }
        public ClientAppServiceInMemoryTest(WarmupInMemory inMemoryData, ITestOutputHelper output)
        {
            _faker = new Faker();
            _output = output;
            InMemoryData = inMemoryData;
            _clientAppService = InMemoryData.Services.GetRequiredService<IClientAppService>();
            _database = InMemoryData.Services.GetRequiredService<JpProjectAdminUiContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            _notifications.Clear();
        }

        [Fact]
        public async Task Should_Add_New_Client()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Include(w => w.Client).Where(w => w.Client.ClientId == command.ClientId).Should().NotBeNull();
        }


        [Fact]
        public async Task Should_Add_New_Client_Without_PostLogout()
        {
            //var command = JsonConvert.DeserializeObject<SaveClientViewModel>("{\"ClientId\":\"aoeu123\",\"ClientName\":\"aoe\",\"LogoUri\":\"https://localhost:5000/storage/1200px-Jenkins_logo.svg.png\",\"ClientType\":\"WebServerSideRenderer\"}");
            var command = JsonConvert.DeserializeObject<SaveClientViewModel>("{\"ClientId\":\"aoeu123\",\"ClientName\":\"aoe\",\"ClientType\":\"WebServerSideRenderer\"}");

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Include(w => w.Client).Where(w => w.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Theory]
        [InlineData(ClientType.Spa, new[] { "openid", "profile" })]
        [InlineData(ClientType.WebHybrid, new[] { "openid", "profile" })]
        [InlineData(ClientType.WebServerSideRenderer, new[] { "openid", "profile" })]
        [InlineData(ClientType.Device, new[] { "openid" })]
        [InlineData(ClientType.Machine, new[] { "openid" })]
        [InlineData(ClientType.Native, new[] { "openid", "profile" })]
        public async Task Should_Add_Default_Scopes(ClientType clientType, string[] scopeName)
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
        [InlineData(ClientType.WebServerSideRenderer)]
        public async Task Should_Add_Cors(ClientType clientType)
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
        [InlineData(ClientType.WebServerSideRenderer)]
        public async Task Should_Add_Default_RedirectUri(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.RedirectUris.Should().Contain(origin => origin.RedirectUri.Equals(command.ClientUri));
        }


        [Fact]
        public async Task Should_Add_ClientServer()
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
        [InlineData(ClientType.WebServerSideRenderer)]
        public async Task Should_Add_Default_LogoutUri_If_Null(ClientType clientType)
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
        [InlineData(ClientType.WebServerSideRenderer)]
        [InlineData(ClientType.Native)]
        public async Task Should_Be_AuthorizationCode_Flow(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.AllowedGrantTypes.Select(s => s.GrantType).Should().Contain("authorization_code");
        }


        [Theory]
        [InlineData(ClientType.Machine)]
        public async Task Should_Be_ClientCredentials_Flow(ClientType clientType)
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientType: clientType).Generate();
            command.LogoutUri = null;

            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().NotBeNull();
            client.AllowedGrantTypes.Select(s => s.GrantType).Should().Contain("client_credentials");
        }

        [Fact]
        public async Task Should_Not_Save_Client_With_PostLogoutUri_With_Trailing_Slash()
        {
            var command = ClientViewModelFaker.GenerateSaveClient(logoutUri: $"{_faker.Internet.Url()}/").Generate();
            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().BeNull();
        }


        [Fact]
        public async Task Should_Not_Save_Client_With_ClientUri_With_TrailingSlash()
        {
            var command = ClientViewModelFaker.GenerateSaveClient(clientUri: $"{_faker.Internet.Url()}/").Generate();
            await _clientAppService.Save(command);

            var client = _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId);
            client.Should().BeNull();
        }
        [Fact]
        public async Task Should_Remove_Client()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Any().Should().BeTrue();

            await _clientAppService.Remove(command.ClientId);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().BeNull();
        }

        [Fact]
        public async Task Should_Update_Client()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientPostLogoutRedirectUris.Any().Should().BeTrue();

            InMemoryData.DetachAll();

            var updateCommand = ClientFaker.GenerateClient().Generate();
            await _clientAppService.Update(command.ClientId, updateCommand);

            var updatedClient = await _clientAppService.GetClientDetails(updateCommand.ClientId);
            updatedClient.AllowedCorsOrigins.Should().Contain(s => updateCommand.AllowedCorsOrigins.Contains(s));
            updatedClient.AllowedCorsOrigins.Count.Should().Be(updatedClient.AllowedCorsOrigins.Count);

            updatedClient.ClientSecrets.Should().Contain(s => updateCommand.ClientSecrets.Contains(s));
            updatedClient.ClientSecrets.Count.Should().Be(updatedClient.ClientSecrets.Count);

            updatedClient.AllowedScopes.Should().Contain(s => updateCommand.AllowedScopes.Contains(s));
            updatedClient.AllowedScopes.Count.Should().Be(updatedClient.AllowedScopes.Count);
        }

        [Fact]
        public async Task Should_Not_Add_Another_Client_With_Same_ClientId()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            var firstCall = await _clientAppService.Save(command);
            var result = await _clientAppService.Save(command);

            firstCall.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
        }

        [Fact]
        public async Task Should_Add_New_ClientSecret()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var secret = ClientViewModelFaker.GenerateSaveClientSecret(command.ClientId).Generate();

            await _clientAppService.SaveSecret(secret);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientSecrets.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Remove_ClientSecret()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var secret = ClientViewModelFaker.GenerateSaveClientSecret(command.ClientId).Generate();

            await _clientAppService.SaveSecret(secret);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            _database.ClientSecrets.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();

            var dbSecret = _database.ClientSecrets.Include(c => c.Client).FirstOrDefault(s => s.Client.ClientId == command.ClientId);

            var commandRemoveSecret = new RemoveClientSecretViewModel(command.ClientId, secret.Type, dbSecret.Value);
            var result = await _clientAppService.RemoveSecret(commandRemoveSecret);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Add_New_ClientSecret_When_Client_Doesnt_Exist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var secret = ClientViewModelFaker.GenerateSaveClientSecret(command.ClientId);

            var result = await _clientAppService.SaveSecret(secret);

            _database.ClientSecrets.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
        }

        [Fact]
        public async Task Should_Add_New_ClientProperty()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveProperty(command.ClientId).Generate();

            await _clientAppService.SaveProperty(property);

            _database.Clients.Include(s => s.Properties).FirstOrDefault(s => s.ClientId == command.ClientId)?.Properties.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Should_Get_ClientProperty()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveProperty(command.ClientId).Generate();

            await _clientAppService.SaveProperty(property);

            _database.Clients.Include(s => s.Properties).FirstOrDefault(s => s.ClientId == command.ClientId)?.Properties.Should().HaveCountGreaterOrEqualTo(1);

            var clientProperties = await _clientAppService.GetProperties(command.ClientId);
            clientProperties.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Should_Not_Add_New_ClientProperty_When_Client_Doesnt_Exist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var property = ClientViewModelFaker.GenerateSaveProperty(command.ClientId).Generate();

            var result = await _clientAppService.SaveProperty(property);

            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);

            _database.ClientProperties.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();

            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
        }

        [Fact]
        public async Task Should_Add_New_ClientClaim()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveClaim(command.ClientId).Generate();

            await _clientAppService.SaveClaim(property);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            var clams = _database.ClientClaims.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId);
            clams.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Should_Get_ClientClaim()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveClaim(command.ClientId).Generate();

            await _clientAppService.SaveClaim(property);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            var clams = await _clientAppService.GetClaims(command.ClientId);
            clams.Should().HaveCountGreaterOrEqualTo(1);
        }

        [Fact]
        public async Task Should_Remove_ClientClaim()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();

            await _clientAppService.Save(command);

            var property = ClientViewModelFaker.GenerateSaveClaim(command.ClientId).Generate();

            await _clientAppService.SaveClaim(property);

            _database.Clients.FirstOrDefault(s => s.ClientId == command.ClientId).Should().NotBeNull();
            var clams = _database.ClientClaims.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId);
            clams.Should().HaveCountGreaterOrEqualTo(1);

            var removeClaim = new RemoveClientClaimViewModel(command.ClientId, property.Type, property.Value);
            await _clientAppService.RemoveClaim(removeClaim);

            var clientDb = await _database.Clients.Include(s => s.Claims).FirstOrDefaultAsync(s => s.ClientId == command.ClientId);
            clientDb.Claims.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Not_Add_New_ClientClaim_When_Client_Doesnt_Exist()
        {
            var command = ClientViewModelFaker.GenerateSaveClient().Generate();
            var property = ClientViewModelFaker.GenerateSaveClaim(command.ClientId).Generate();

            var result = await _clientAppService.SaveClaim(property);

            _database.ClientClaims.Include(i => i.Client).Where(f => f.Client.ClientId == command.ClientId).Should().NotBeNull();
            result.Should().BeFalse(becauseArgs: _notifications.GetNotificationsByKey());
        }

    }
}
