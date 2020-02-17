using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Bus;
using JPProject.Domain.Core.Interfaces;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.CommandHandlers;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Fakers.Test.Email;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Domain.Tests.CommandHandlers.EmailTests
{
    public class EmailCommandHandlerTests
    {
        private Faker _faker;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IMediatorHandler> _mediator;
        private readonly Mock<DomainNotificationHandler> _notifications;
        private readonly EmailCommandHandler _commandHandler;
        private readonly Mock<ITemplateRepository> _templateRepository;
        private readonly Mock<IEmailRepository> _emailRepository;

        public EmailCommandHandlerTests()
        {
            _faker = new Faker();
            _uow = new Mock<IUnitOfWork>();
            _mediator = new Mock<IMediatorHandler>();
            _notifications = new Mock<DomainNotificationHandler>();
            _templateRepository = new Mock<ITemplateRepository>();
            _emailRepository = new Mock<IEmailRepository>();
            _commandHandler = new EmailCommandHandler(_uow.Object, _mediator.Object, _notifications.Object, _templateRepository.Object, _emailRepository.Object);
        }

        [Fact]
        public async Task Should_Save_Template()
        {
            var template = EmailCommandFaker.GenerateSaveTemplateCommand("test-email-ok").Generate();

            _templateRepository.Setup(s => s.Add(It.Is<Template>(m => !string.IsNullOrEmpty(m.Username))));
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(template, CancellationToken.None);

            _templateRepository.Verify(v => v.Exist(It.IsAny<string>()), Times.Once);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Not_Save_Template_When_Name_Already_Exist()
        {
            var template = EmailCommandFaker.GenerateSaveTemplateCommand().Generate();

            _templateRepository.Setup(s => s.Exist(It.Is<string>(m => m == template.Name))).ReturnsAsync(true);

            var result = await _commandHandler.Handle(template, CancellationToken.None);

            _uow.Verify(v => v.Commit(), Times.Never);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Not_Save_Template_When_Name_Contains_Blank_Space()
        {
            var template = EmailCommandFaker.GenerateSaveTemplateCommand(name: "Teste with blank space").Generate();

            var result = await _commandHandler.Handle(template, CancellationToken.None);

            _templateRepository.Verify(s => s.Exist(It.IsAny<string>()), Times.Never());
            _uow.Verify(v => v.Commit(), Times.Never);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_Update_Template()
        {
            var template = EmailCommandFaker.GenerateUpdateTemplateCommand().Generate();

            _templateRepository.Setup(s => s.GetByName(It.Is<string>(m => m == template.OldName))).ReturnsAsync(EmailFaker.GenerateTemplate());
            _templateRepository.Setup(s => s.Update(It.Is<Template>(m => !string.IsNullOrEmpty(m.Username) && m.Name == template.Name)));
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(template, CancellationToken.None);

            _templateRepository.Verify(v => v.GetByName(It.IsAny<string>()), Times.Once);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Save_Email_When_Type_Not_Found()
        {
            var emailCommand = EmailCommandFaker.GenerateSaveEmailCommand().Generate();

            _emailRepository.Setup(s => s.GetByType(It.IsAny<EmailType>())).ReturnsAsync((Email)null);
            _emailRepository.Setup(s => s.Add(It.IsAny<Email>()));
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(emailCommand, CancellationToken.None);

            _emailRepository.Verify(s => s.Add(It.IsAny<Email>()), Times.Once);

            result.Should().BeTrue();
        }


        [Fact]
        public async Task Should_Update_Email_When_Type_Found()
        {
            var emailCommand = EmailCommandFaker.GenerateSaveEmailCommand().Generate();

            _emailRepository.Setup(s => s.GetByType(It.IsAny<EmailType>())).ReturnsAsync(EmailFaker.GenerateEmail().Generate());
            _emailRepository.Setup(s => s.Update(It.IsAny<Email>()));
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            var result = await _commandHandler.Handle(emailCommand, CancellationToken.None);

            _emailRepository.Verify(s => s.Update(It.IsAny<Email>()), Times.Once);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task Shoulh_Remove_Template()
        {
            var emailCommand = EmailCommandFaker.GenerateRemoveTemplateCommand().Generate();
            var templateToRemove = EmailFaker.GenerateTemplate().Generate();
            _uow.Setup(s => s.Commit()).ReturnsAsync(true);

            _templateRepository.Setup(s => s.GetByName(It.Is<string>(m => m == emailCommand.Name))).ReturnsAsync(templateToRemove);
            _templateRepository.Setup(s => s.Remove(It.Is<Template>(m => m.Id == templateToRemove.Id)));

            var result = await _commandHandler.Handle(emailCommand, CancellationToken.None);

            _templateRepository.Verify(v => v.Remove(It.IsAny<Template>()), Times.Once);

            result.Should().BeTrue();
        }
    }
}

