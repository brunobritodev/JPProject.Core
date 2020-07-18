using Bogus;
using FluentAssertions;
using IdentityServer4.Models;
using JPProject.Admin.Domain.CommandHandlers;
using JPProject.Admin.Domain.Commands.ApiScope;
using JPProject.Admin.Domain.Interfaces;
using JPProject.Admin.Fakers.Test.ApiScopeFakers;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Admin.Domain.Tests.CommandHandlers.ApiScopeTests
{
    public class ApiScopeCommandHandlerTests
    {
        private readonly ApiScopeCommandHandler _commandHandler;
        private readonly Mock<DomainNotificationHandler> _notifications;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IApiScopeRepository> _apiScopeRepository;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Faker _faker;
        public ApiScopeCommandHandlerTests()
        {
            _faker = new Faker();
            _tokenSource = new CancellationTokenSource();
            _uow = new Mock<IUnitOfWork>();
            _mediator = new Mock<IMediatorHandler>();
            _notifications = new Mock<DomainNotificationHandler>();
            _apiScopeRepository = new Mock<IApiScopeRepository>();
            _commandHandler = new ApiScopeCommandHandler(_uow.Object, _mediator.Object, _notifications.Object, _apiScopeRepository.Object);
        }

        [Fact]
        public async Task Should_Not_Save_Scope_When_It_Already_Exist()
        {
            var command = ApiScopeCommandFaker.GenerateSaveApiScopeCommand().Generate();

            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == command.ApiScope.Name))).ReturnsAsync(ApiScopeFaker.GenerateApiScope().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            result.Should().BeFalse();
            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == command.ApiScope.Name)), Times.Once);
            _uow.Verify(v => v.Commit(), Times.Never);

        }

        [Fact]
        public async Task Should_Save_Scope()
        {
            var command = ApiScopeCommandFaker.GenerateSaveApiScopeCommand().Generate();
            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == command.ApiScope.Name))).ReturnsAsync((ApiScope)null);
            _apiScopeRepository.Setup(s => s.Add(It.Is<ApiScope>(i => i.Name == command.ApiScope.Name)));
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _apiScopeRepository.Verify(s => s.Add(It.IsAny<ApiScope>()), Times.Once);
            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == command.ApiScope.Name)), Times.Once);

            result.Should().BeTrue();
        }


        [Fact]
        public async Task Should_Not_Update_Scope_When_It_Doesnt_Exist()
        {
            var command = ApiScopeCommandFaker.GenerateUpdateApiScopeCommand().Generate();
            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == command.OldName))).ReturnsAsync(ApiScopeFaker.GenerateApiScope().Generate());

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            result.Should().BeFalse();
            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == command.OldName)), Times.Once);
        }


        [Fact]
        public async Task Should_Not_Update_Scope_When_Name_Isnt_Provided()
        {
            var command = ApiScopeCommandFaker.GenerateUpdateApiScopeCommand(setName: false).Generate();

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            result.Should().BeFalse();
            _uow.Verify(v => v.Commit(), Times.Never);

        }

        [Fact]
        public async Task Should_Update_Scope()
        {
            var oldResourceName = "old-resource-name";
            var command = ApiScopeCommandFaker.GenerateUpdateApiScopeCommand(oldName: oldResourceName).Generate();
            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == oldResourceName))).ReturnsAsync(ApiScopeFaker.GenerateApiScope().Generate());
            _apiScopeRepository.Setup(s => s.UpdateWithChildren(It.Is<string>(s => s == command.OldName), It.Is<ApiScope>(i => i.Name == command.ApiScope.Name))).Returns(Task.CompletedTask);
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _apiScopeRepository.Verify(s => s.UpdateWithChildren(It.Is<string>(s => s == command.OldName), It.IsAny<ApiScope>()), Times.Once);
            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == oldResourceName)), Times.Once);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Remove_Scope_When_Name_Isnt_Provided()
        {
            var command = new RemoveApiScopeCommand(null);
            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            result.Should().BeFalse();
            _uow.Verify(v => v.Commit(), Times.Never);
        }


        [Fact]
        public async Task Should_Not_Remove_Scope_When_It_Doesnt_Exist()
        {
            var command = ApiScopeCommandFaker.GenerateUpdateApiScopeCommand().Generate();

            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == command.OldName))).ReturnsAsync((ApiScope)null);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);


            result.Should().BeFalse();
            _uow.Verify(v => v.Commit(), Times.Never);
            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == command.OldName)), Times.Once);
        }

        [Fact]
        public async Task Should_Remove_Resource()
        {
            var command = ApiScopeCommandFaker.GenerateRemoveApiScopeCommand().Generate();
            _apiScopeRepository.Setup(s => s.Get(It.Is<string>(q => q == command.ResourceName))).ReturnsAsync(ApiScopeFaker.GenerateApiScope().Generate());
            _apiScopeRepository.Setup(s => s.RemoveScope(It.IsAny<string>()));

            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(command, _tokenSource.Token);

            _apiScopeRepository.Verify(s => s.Get(It.Is<string>(q => q == command.ResourceName)), Times.Once);
            _apiScopeRepository.Verify(s => s.RemoveScope(It.IsAny<string>()), Times.Once);

            result.Should().BeTrue();
        }
    }
}
