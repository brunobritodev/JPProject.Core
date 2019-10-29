using Bogus;
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
        private readonly ApplicationIdentityContext _database;
        private readonly Faker _faker;
        public WarmupInMemory InMemoryData { get; }

        public UserAppServiceInMemoryTests(WarmupInMemory inMemory)
        {
            _faker = new Faker();
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

        [Fact]
        public async Task ShouldNotRegisterDuplicatedUsername()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();

            var username = command.Username;
            command = UserViewModelFaker.GenerateUserViewModel().Generate();
            command.Username = username;

            result = await _userAppService.Register(command);
            result.Should().BeFalse();
            _database.Users.Count(f => f.UserName == command.Username).Should().Be(1);
        }

        [Fact]
        public async Task ShouldNotRegisterDuplicatedEmail()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.Email == command.Email).Should().NotBeNull();

            var email = command.Email;
            command = UserViewModelFaker.GenerateUserViewModel().Generate();
            command.Email = email;

            result = await _userAppService.Register(command);
            result.Should().BeFalse();
            _database.Users.Count(f => f.Email == command.Email).Should().Be(1);
        }


        [Fact]
        public async Task ShouldNotRegisterUserWithoutPassword()
        {
            var command = UserViewModelFaker.GenerateUserWithProviderViewModel().Generate();
            command.Password = null;
            command.ConfirmPassword = null;

            var result = await _userAppService.Register(command);
            result.Should().BeFalse();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().BeNull();
        }

        [Fact]
        public async Task ShouldRegisterNewUserWithoutPassword()
        {
            var command = UserViewModelFaker.GenerateSocialViewModel().Generate();

            var result = await _userAppService.RegisterWithoutPassword(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldRegisterNewUserWithProviderAndWithoutPassword()
        {
            var command = UserViewModelFaker.GenerateUserWithProviderViewModel().Generate();
            command.Password = null;
            command.ConfirmPassword = null;


            var result = await _userAppService.RegisterWithProvider(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldNotFindUser()
        {
            var result = await _userAppService.FindByUsernameAsync(_faker.Person.FirstName);
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotFindUsername()
        {
            var result = await _userAppService.FindByUsernameAsync(_faker.Person.UserName);
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldNotFindEmail()
        {
            var result = await _userAppService.FindByEmailAsync(_faker.Person.Email);
            result.Should().BeNull();
        }

        [Fact]
        public async Task ShouldUsernameExist()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var result = await _userAppService.CheckUsername(command.Username);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldEmailExist()
        {

            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var result = await _userAppService.CheckEmail(command.Email);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ShouldUsernameNotExist()
        {
            var result = await _userAppService.CheckUsername(_faker.Person.UserName);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldEmailNotExist()
        {
            var result = await _userAppService.CheckEmail(_faker.Person.Email);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldFindUsername()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);
            var result = await _userAppService.FindByUsernameAsync(command.Username);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldFindEmail()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);
            var result = await _userAppService.FindByEmailAsync(command.Email);
            result.Should().NotBeNull();
        }


        [Fact]
        public async Task ShouldAddLogin()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var socialUser = UserViewModelFaker.GenerateSocialViewModel(command.Email, command.Username).Generate();
            socialUser.Email = command.Email;
            socialUser.Username = command.Username;

            var result = await _userAppService.AddLogin(socialUser);
            result.Should().BeTrue();
            var userId = await _userAppService.FindByEmailAsync(command.Email);

            _database.UserLogins.Any(s => s.UserId == userId.Id).Should().BeTrue();
        }

        [Fact]
        public async Task ShouldFindLoginByProvider()
        {
            var command = UserViewModelFaker.GenerateSocialViewModel().Generate();

            var result = await _userAppService.RegisterWithoutPassword(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();

            var user = await _userAppService.FindByProviderAsync(command.Provider, command.ProviderId);
            user.Should().NotBeNull();
        }
    }
}
