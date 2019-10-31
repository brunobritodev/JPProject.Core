using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Domain.Core.StringUtils;
using JPProject.Sso.Domain.CommandHandlers;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Fakers.Test.Users;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Domain.Tests.UserTests
{
    public class UserManagerCommandHandlerTests
    {
        private Faker _faker;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly Mock<DomainNotificationHandler> _notifications;
        private readonly UserManagementCommandHandler _commandHandler;
        private readonly Mock<IUserService> _userService;
        private readonly Mock<ISystemUser> _systemUser;

        public UserManagerCommandHandlerTests()
        {
            _faker = new Faker();
            _tokenSource = new CancellationTokenSource();
            _uow = new Mock<IUnitOfWork>();
            _mediator = new Mock<IMediatorHandler>();
            _notifications = new Mock<DomainNotificationHandler>();
            _userService = new Mock<IUserService>();
            _systemUser = new Mock<ISystemUser>();
            _commandHandler = new UserManagementCommandHandler(_uow.Object, _mediator.Object, _notifications.Object, _userService.Object, _systemUser.Object);

        }

        [Fact]
        public async Task ShouldRemoveClaimWithoutValue()
        {
            var user = UserFaker.GenerateUser().Generate();
            var command = UserManagerCommandFaker.GenerateRemoveClaimCommand(false).Generate();

            _userService.Setup(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username))).ReturnsAsync(user);
            _userService.Setup(s => s.RemoveClaim(
                       It.Is<string>(c => c == user.Id),
                     It.Is<string>(c => c.Equals(command.Type)),
                        It.Is<string>(c => c.IsMissing())))
                .ReturnsAsync(true);

            _uow.Setup(s => s.Commit()).Returns(true);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _userService.Verify(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username)), Times.Once);

            result.Should().BeTrue();
        }


        [Fact]
        public async Task ShouldRemoveClaimWithValue()
        {
            var user = UserFaker.GenerateUser().Generate();
            var command = UserManagerCommandFaker.GenerateRemoveClaimCommand().Generate();

            _userService.Setup(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username))).ReturnsAsync(user);
            _userService.Setup(s => s.RemoveClaim(
                    It.Is<string>(c => c == user.Id),
                    It.Is<string>(c => c.Equals(command.Type)),
                    It.Is<string>(c => c.Equals(command.Value))))
                .ReturnsAsync(true);

            _uow.Setup(s => s.Commit()).Returns(true);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _userService.Verify(s => s.FindByNameAsync(It.Is<string>(e => e == command.Username)), Times.Once);

            result.Should().BeTrue();
        }



        [Fact]
        public void ShouldNotContainsFederationGateway()
        {
            var user = UserViewModelFaker.GenerateUserViewModel().Generate();

            user.ContainsFederationGateway().Should().BeFalse();
        }

    }
}