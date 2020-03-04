using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels.ApiResouceViewModels;
using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.Admin.Fakers.Test.ApiResourceFakers;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Admin.IntegrationTests.ApiResourceTests
{
    public class ApiResourceAppServiceInMemoryTest : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly IApiResourceAppService _apiResourceAppService;
        private readonly JpProjectAdminUiContext _database;
        private DomainNotificationHandler _notifications;
        public WarmupInMemory InMemoryData { get; }

        public ApiResourceAppServiceInMemoryTest(WarmupInMemory inMemoryData, ITestOutputHelper output)
        {
            _output = output;
            InMemoryData = inMemoryData;
            _apiResourceAppService = InMemoryData.Services.GetRequiredService<IApiResourceAppService>();
            _database = InMemoryData.Services.GetRequiredService<JpProjectAdminUiContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            _notifications.Clear();
        }

        [Fact]
        public async Task Should_Add_New_Api()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var api = _database.ApiResources.FirstOrDefault(s => s.Name == command.Name);
            api.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_ApiResource_Have_At_Least_One_Scope()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var api = _database.ApiResources.FirstOrDefault(s => s.Name == command.Name);
            api.Should().NotBeNull();
            api.Scopes.Should().HaveCountGreaterOrEqualTo(1);
            api.Scopes.Should().Contain(a => a.Name == api.Name);
        }


        [Fact]
        public async Task Should_ApiResource_Have_At_Least_One_Scope_With_His_Name_When_Register()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var api = _database.ApiResources.FirstOrDefault(s => s.Name == command.Name);
            api.Should().NotBeNull();
            api.Scopes.Should().HaveCountGreaterOrEqualTo(1);
            api.Scopes.Should().Contain(a => a.Name == api.Name);
        }

        [Fact]
        public async Task Should_Remove_Api()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            _database.ApiResources.FirstOrDefault(s => s.Name == command.Name).Should().NotBeNull();

            await _apiResourceAppService.Remove(new RemoveApiResourceViewModel(command.Name));

            _database.ApiResources.FirstOrDefault(s => s.Name == command.Name).Should().BeNull();
        }

        [Fact]
        public async Task Should_Update_Api()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var client = _database.ApiResources.FirstOrDefault(s => s.Name == command.Name);
            client.Should().NotBeNull();

            InMemoryData.DetachAll();

            var updateCommand = ApiResourceFaker.GenerateApiResource().Generate();
            await _apiResourceAppService.Update(command.Name, updateCommand);

            _database.ApiResources.FirstOrDefault(f => f.Name == updateCommand.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Not_Add_Another_Api_With_Same_Name()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            var firstCall = await _apiResourceAppService.Save(command);
            var result = await _apiResourceAppService.Save(command);
            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);
            firstCall.Should().BeTrue();
            result.Should().BeFalse();
        }


        [Fact]
        public async Task Should_Add_New_ApiScope()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var scope = ApiResourceFaker.GenerateSaveScopeViewModer(command.Name).Generate();

            await _apiResourceAppService.SaveScope(scope);

            _database.ApiResources.FirstOrDefault(f => f.Name == command.Name).Should().NotBeNull();
            _database.ApiScopes.Include(i => i.ApiResource).Where(f => f.ApiResource.Name == command.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Add_New_ApiSecret()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();

            await _apiResourceAppService.Save(command);

            var secret = ApiResourceFaker.GenerateSaveClientSecret(command.Name);

            await _apiResourceAppService.SaveSecret(secret);

            _database.ApiResources.FirstOrDefault(f => f.Name == command.Name).Should().NotBeNull();
            _database.ApiSecrets.Include(i => i.ApiResource).Where(f => f.ApiResource.Name == command.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Not_Add_New_ApiSecret_When_Client_Doesnt_Exist()
        {
            var command = ApiResourceFaker.GenerateApiResource().Generate();
            var secret = ApiResourceFaker.GenerateSaveClientSecret(command.Name).Generate();

            var result = await _apiResourceAppService.SaveSecret(secret);

            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);
            _database.ApiSecrets.Include(i => i.ApiResource).Where(f => f.ApiResource.Name == command.Name).Should().NotBeNull();
            result.Should().BeFalse();
        }


    }
}
