using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.CommandHandlers;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Fakers.Test.Users;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Domain.Tests.CommandHandlers.UserTests
{
    public class UserCommandHandlerTests
    {
        private readonly Faker _faker;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly Mock<DomainNotificationHandler> _notifications;
        private readonly UserCommandHandler _commandHandler;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IEmailRepository> _emailRepository;

        public UserCommandHandlerTests()
        {
            _faker = new Faker();
            _tokenSource = new CancellationTokenSource();
            _uow = new Mock<IUnitOfWork>();
            _mediator = new Mock<IMediatorHandler>();
            _notifications = new Mock<DomainNotificationHandler>();
            _userService = new Mock<IUserService>();
            _emailService = new Mock<IEmailService>();
            _emailRepository = new Mock<IEmailRepository>();
            _commandHandler = new UserCommandHandler(_uow.Object, _mediator.Object, _notifications.Object, _userService.Object, _emailService.Object, _emailRepository.Object);

        }

        [Fact]
        public async Task ShouldNotAddNewUser_AfterSuccessfulLoginThrough_ExternalProvider_IfHisEmailAlreadyExist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserWithoutPassCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task ShouldNotAddNewUser_AfterSuccessfulLoginThrough_ExternalProvider_IfHisNameAlreadyExist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserWithoutPassCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync((User)null);
            _userService.Setup(s => s.FindByNameAsync(It.Is<string>(n => n == command.Username))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            _userService.Verify(s => s.FindByNameAsync(It.Is<string>(n => n == command.Username)), Times.Once);
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldNotAddLoginIfUserDoesntExist()
        {
            var command = UserCommandFaker.GenerateAddLoginCommand().Generate();

            _userService.Setup(s => s.AddLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((string)null);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _userService.Verify(s => s.AddLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task ShouldNotAddNewUserIfItEmailAlreadyExist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task ShouldNotAddNewUserIfItNameAlreadyExist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync((User)null);
            _userService.Setup(s => s.FindByNameAsync(It.Is<string>(n => n == command.Username))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            _userService.Verify(s => s.FindByNameAsync(It.Is<string>(n => n == command.Username)), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task ShouldNotAddNewUserIfPasswordDifferFromConfirmation()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand(_faker.Internet.Password()).Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldNotGenerateResetLinkIfNotProvideUsernameOrEmail()
        {
            var command = UserCommandFaker.GenerateSendResetLinkCommand().Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            result.Should().BeFalse();
        }

        [Fact]
        public async Task ShouldNotRegisterUserWithAFutureBirthdate()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand(birthdate: _faker.Date.Future()).Generate();

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
        }
    }
}
