using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Infra.Data.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Integration.Tests.UserTests
{
    public class UserAppServiceInMemoryTests : IClassFixture<WarmupInMemory>
    {
        private readonly IUserAppService _userAppService;
        private ApplicationIdentityContext _database;
        public WarmupInMemory InMemoryData { get; }

        public UserAppServiceInMemoryTests(WarmupInMemory inMemory)
        {
            InMemoryData = inMemory;
            _userAppService = InMemoryData.Services.GetRequiredService<IUserAppService>();
            _database = InMemoryData.Services.GetRequiredService<ApplicationIdentityContext>();
            var notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            notifications.Clear();
        }

        [Fact]
        public async Task ShouldRegisterNewUser()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

    }
}
