using Bogus;
using FluentAssertions;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.Admin.Fakers.Test.ApiScopeFakers;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Admin.IntegrationTests.ApiScopeTests
{
    public class ApiScopeAppServiceInMemoryTest : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly IApiScopeAppService _ApiScopeAppService;
        private readonly JpProjectAdminUiContext _database;
        private readonly DomainNotificationHandler _notifications;
        private Faker _faker;
        public WarmupInMemory InMemoryData { get; }

        public ApiScopeAppServiceInMemoryTest(WarmupInMemory inMemoryData, ITestOutputHelper output)
        {
            _output = output;
            InMemoryData = inMemoryData;
            _ApiScopeAppService = InMemoryData.Services.GetRequiredService<IApiScopeAppService>();
            _database = InMemoryData.Services.GetRequiredService<JpProjectAdminUiContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            _notifications.Clear();
            _faker = new Faker();
        }

        [Fact]
        public async Task Should_Add_New_Api()
        {
            var command = ApiScopeFaker.GenerateApiScope().Generate();

            await _ApiScopeAppService.Save(command);

            var api = _database.ApiScopes.FirstOrDefault(s => s.Name == command.Name);
            api.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Remove_ApiScope()
        {
            var command = ApiScopeFaker.GenerateApiScope().Generate();

            await _ApiScopeAppService.Save(command);

            _database.ApiScopes.FirstOrDefault(s => s.Name == command.Name).Should().NotBeNull();

            await _ApiScopeAppService.Remove(command.Name);

            _database.ApiScopes.FirstOrDefault(s => s.Name == command.Name).Should().BeNull();
        }

        [Fact]
        public async Task Should_Update_ApiScope()
        {
            var command = ApiScopeFaker.GenerateApiScope().Generate();

            await _ApiScopeAppService.Save(command);

            var client = _database.ApiScopes.FirstOrDefault(s => s.Name == command.Name);
            client.Should().NotBeNull();

            InMemoryData.DetachAll();

            var updateCommand = ApiScopeFaker.GenerateApiScope().Generate();
            await _ApiScopeAppService.Update(command.Name, updateCommand);

            _database.ApiScopes.FirstOrDefault(f => f.Name == updateCommand.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Not_Add_Another_Api_With_Same_Name()
        {
            var command = ApiScopeFaker.GenerateApiScope().Generate();

            var firstCall = await _ApiScopeAppService.Save(command);
            var result = await _ApiScopeAppService.Save(command);
            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);
            firstCall.Should().BeTrue();
            result.Should().BeFalse();
        }


        [Fact]
        public async Task Should_Add_New_ApiScope()
        {
            var command = ApiScopeFaker.GenerateApiScope().Generate();

            await _ApiScopeAppService.Save(command);

            command.UserClaims = new List<string>() { "new_scope" };

            await _ApiScopeAppService.Update(command.Name, command);

            var resourceDb = _database.ApiScopes.Include(s => s.UserClaims).FirstOrDefault(f => f.Name == command.Name);
            resourceDb.Should().NotBeNull();
            _database.ApiScopeClaims.Include(i => i.Scope).Where(f => f.Scope.Name == command.Name).Should().NotBeNull();
            resourceDb.UserClaims.Any(a => a.Type.Equals("new_scope")).Should().BeTrue();

        }
    }
}
