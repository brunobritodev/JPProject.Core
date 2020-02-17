using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels.UserViewModels;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.UserTests
{
    public abstract class UserAppServiceAbstractTests<T> : IClassFixture<T> where T : class, IWarmupTest
    {
        private readonly ITestOutputHelper _output;
        private readonly IUserAppService _userAppService;
        private readonly UnifiedContext _database;
        private readonly Faker _faker;
        private readonly IUserManageAppService _userManagerAppService;
        private readonly DomainNotificationHandler _notifications;
        public T UnifiedContextData { get; }

        protected UserAppServiceAbstractTests(T unifiedContext, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            UnifiedContextData = unifiedContext;
            _userAppService = UnifiedContextData.Services.GetRequiredService<IUserAppService>();
            _userManagerAppService = UnifiedContextData.Services.GetRequiredService<IUserManageAppService>();
            _database = UnifiedContextData.Services.GetRequiredService<UnifiedContext>();
            _notifications = (DomainNotificationHandler)UnifiedContextData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }

        [Fact]
        public async Task Should_Register_New_User()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Update_User()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();

            var user = await _userManagerAppService.GetUserDetails(command.Username);
            user.Birthdate = DateTime.Now.Date.AddYears(-18);
            result = await _userManagerAppService.UpdateUser(user);

            result.Should().BeTrue();

            user = await _userManagerAppService.GetUserDetails(command.Username);
            user.Birthdate.Should().Be(DateTime.Now.Date.AddYears(-18));
        }

        [Fact]
        public async Task Should_Save_SocialNumber()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username)?.SocialNumber.Should().NotBeNull();
        }


        [Fact]
        public async Task User_LockoutDate_Should_Be_Null()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();

            var savedUser = await _userManagerAppService.GetUserDetails(command.Username);
            savedUser.LockoutEnd.Should().BeNull();
        }


        [Fact]
        public async Task Should_Not_Register_Duplicated_Username()
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
        public async Task Should_Not_Register_Duplicated_Email()
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
        public async Task Should_Not_Register_User_Without_Password()
        {
            var command = UserViewModelFaker.GenerateUserWithProviderViewModel().Generate();
            command.Password = null;
            command.ConfirmPassword = null;

            var result = await _userAppService.Register(command);
            result.Should().BeFalse();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().BeNull();
        }

        [Fact]
        public async Task Should_Register_New_User_Without_Password()
        {
            var command = UserViewModelFaker.GenerateSocialViewModel().Generate();

            var result = await _userAppService.RegisterWithoutPassword(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Register_New_User_With_Provider_And_Without_Password()
        {
            var command = UserViewModelFaker.GenerateUserWithProviderViewModel().Generate();
            command.Password = null;
            command.ConfirmPassword = null;


            var result = await _userAppService.RegisterWithProvider(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Not_Find_User()
        {
            var result = await _userManagerAppService.FindByUsernameAsync(_faker.Person.FirstName);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_Not_Find_Username()
        {
            var result = await _userManagerAppService.FindByUsernameAsync(_faker.Person.UserName);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_Not_Find_Email()
        {
            var result = await _userManagerAppService.FindByEmailAsync(_faker.Person.Email);
            result.Should().BeNull();
        }

        [Fact]
        public async Task Should_Username_Exist()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var result = await _userAppService.CheckUsername(command.Username);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Email_Exist()
        {

            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var result = await _userAppService.CheckEmail(command.Email);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Username_Not_Exist()
        {
            var result = await _userAppService.CheckUsername(_faker.Person.UserName);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Email_Not_Exist()
        {
            var result = await _userAppService.CheckEmail(_faker.Person.Email);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Find_Username()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);
            var result = await _userManagerAppService.FindByUsernameAsync(command.Username);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Find_Email()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);
            var result = await _userManagerAppService.FindByEmailAsync(command.Email);
            result.Should().NotBeNull();
        }


        [Fact]
        public async Task Should_Add_Login()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var socialUser = UserViewModelFaker.GenerateSocialViewModel(command.Email, command.Username).Generate();
            socialUser.Email = command.Email;
            socialUser.Username = command.Username;

            var result = await _userAppService.AddLogin(socialUser);
            result.Should().BeTrue();
            var userId = await _userManagerAppService.FindByEmailAsync(command.Email);

            var user = _database.Users.First(f => f.UserName == userId.UserName);
            _database.UserLogins.Any(s => s.UserId == user.Id).Should().BeTrue();
        }

        [Fact]
        public async Task Should_Find_Login_By_Provider()
        {
            var command = UserViewModelFaker.GenerateSocialViewModel().Generate();

            var result = await _userAppService.RegisterWithoutPassword(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();

            var user = await _userManagerAppService.FindByProviderAsync(command.Provider, command.ProviderId);
            user.Should().NotBeNull();
        }


        [Fact]
        public async Task Should_Find_By_Ids()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);
            command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var users = _database.Users.Select(s => s.Id).ToArray();


            var search = new UserSearch() { Id = users };

            var usersFound = await _userManagerAppService.SearchUsers(search);

            usersFound.Total.Should().BeGreaterOrEqualTo(1);
            usersFound.Collection.ToList().Count.Should().Be(usersFound.Total);

        }

        [Fact]
        public async Task Should_Send_Reset_Link_By_Username()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);

            var emailSent = await _userAppService.SendResetLink(new ForgotPasswordViewModel(command.Username));
            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);

            emailSent.Should().BeTrue();

        }

        [Fact]
        public async Task Should_Send_Reset_Link_By_Email()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            await _userAppService.Register(command);


            var emailSent = await _userAppService.SendResetLink(new ForgotPasswordViewModel(command.Email));
            _notifications.GetNotifications().Select(s => s.Value).ToList().ForEach(_output.WriteLine);

            emailSent.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Register_New_User_With_LockOut_Disabled()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            var user = _database.Users.FirstOrDefault(f => f.UserName == command.Username);
            user.Should().NotBeNull();
            user.LockoutEnd.Should().BeNull();
        }



        [Fact]
        public async Task Should_Search_User_By_Name()
        {
            string name = string.Empty;
            var commands = UserViewModelFaker.GenerateUserViewModel().Generate(_faker.Random.Int(1, 10));
            foreach (var command in commands)
            {
                var result = await _userAppService.Register(command);
                result.Should().BeTrue();
                name = command.Name;
            }

            var search = new UserFindByEmailNameUsername(name);

            var users = await _userManagerAppService.SearchUsers(search);

            users.Total.Should().BeGreaterOrEqualTo(1);
            users.Collection.ToList().Count.Should().Be(users.Total);
        }

        [Fact]
        public async Task Should_Search_User_By_Ssn()
        {
            string ssn = string.Empty;
            var commands = UserViewModelFaker.GenerateUserViewModel().Generate(_faker.Random.Int(1, 10));

            foreach (var command in commands)
            {
                var result = await _userAppService.Register(command);
                result.Should().BeTrue();
                ssn = command.SocialNumber;
            }

            var search = new UserSearch() { Ssn = ssn };

            var users = await _userManagerAppService.SearchUsers(search);

            users.Total.Should().BeGreaterOrEqualTo(1);
            users.Collection.ToList().Count.Should().Be(users.Total);
        }

    }

    [Trait("Category", "Database - Unified Contexts")]
    public class UserUnifiedContextTests : UserAppServiceAbstractTests<WarmupUnifiedContext>
    {
        public UserUnifiedContextTests(WarmupUnifiedContext unifiedContext, ITestOutputHelper output) : base(unifiedContext, output)
        {
        }
    }

    [Trait("Category", "Database - Separeted Contexts")]
    public class UserManyContextTests : UserAppServiceAbstractTests<WarmupUnifiedContext>
    {
        public UserManyContextTests(WarmupUnifiedContext unifiedContext, ITestOutputHelper output) : base(unifiedContext, output)
        {
        }
    }
}
