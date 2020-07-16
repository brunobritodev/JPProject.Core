using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.CommandHandlers;
using JPProject.Sso.Domain.Commands.User;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Fakers.Test.Email;
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
        public async Task Should_Not_Add_New_User_After_Successful_Login_Through_External_Provider_If_His_Email_Already_Exist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserWithoutPassCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task Should_Not_Add_New_User_After_Successful_Login_Through_ExternalProvider_If_His_Name_Already_Exist()
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
        public async Task Should_Not_Add_Login_If_User_Doesnt_Exist()
        {
            var command = UserCommandFaker.GenerateAddLoginCommand().Generate();

            _userService.SetupSequence(s => s.AddLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string)null)
                .ReturnsAsync((string)null);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _userService.Verify(s => s.AddLoginAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            result.Should().BeFalse();
        }



        [Fact]
        public async Task Should_Not_Add_New_User_If_It_Email_Already_Exist()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand().Generate();

            _userService.Setup(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email))).ReturnsAsync(UserFaker.GenerateUser().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            result.Should().BeFalse();
        }


        [Fact]
        public async Task Should_Add_New_User_If_It_Dont_Have_Email()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserWithoutPassCommand(checkProvider: false, setEmail: false, setName: false).Generate();

            _userService.SetupSequence(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username)))
                .ReturnsAsync((IDomainUser)null)
                .ReturnsAsync(UserFaker.GenerateUser().Generate());

            _userService.Setup(s =>
                s.CreateUserWithouthPassword(
                    It.Is<RegisterNewUserWithoutPassCommand>(i => i.Username.Equals(command.Username) && i.Email == null))
                )
                .ReturnsAsync(
                    new AccountResult(command.Username, _faker.Random.AlphaNumeric(8), _faker.Internet.Url())
                    );

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            _userService.Verify(s => s.FindByEmailAsync(It.Is<string>(e => e == command.Email)), Times.Once);
            _userService.Verify(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username)), Times.Exactly(2));
            result.Should().BeTrue();
        }


        [Fact]
        public async Task Should_Not_Add_New_User_If_It_Name_Already_Exist()
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
        public async Task Should_Not_Add_New_User_If_Password_Differ_From_Confirmation()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand(_faker.Internet.Password()).Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Not_Generate_Reset_Link_If_Not_Provide_Username_Or_Email()
        {
            var command = UserCommandFaker.GenerateSendResetLinkCommand().Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Not_Register_User_With_A_Future_Birthdate()
        {
            var command = UserCommandFaker.GenerateRegisterNewUserCommand(birthdate: _faker.Date.Future()).Generate();

            var result = await _commandHandler.Handle(command, CancellationToken.None);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Send_Email_After_Successfull_Registration()
        {
            _emailRepository.Setup(s => s.GetByType(It.IsAny<EmailType>())).ReturnsAsync(EmailFaker.GenerateEmail());
            _userService.Setup(s => s.CreateUserWithPass(It.IsAny<RegisterNewUserCommand>(), It.IsAny<string>())).ReturnsAsync(new AccountResult(_faker.Random.Guid().ToString(), _faker.Random.String(), _faker.Internet.Url()));
            _userService.SetupSequence(s => s.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((IDomainUser)null).ReturnsAsync(UserFaker.GenerateUser(confirmedEmail: false).Generate());

            var command = UserCommandFaker.GenerateRegisterNewUserCommand(shouldConfirmEmail: true).Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            result.Should().BeTrue();
            _userService.Verify(s => s.CreateUserWithPass(It.IsAny<RegisterNewUserCommand>(), It.IsAny<string>()), Times.Once);
            _emailService.Verify(e => e.SendEmailAsync(It.IsAny<EmailMessage>()), Times.Once);
        }
    }
}
