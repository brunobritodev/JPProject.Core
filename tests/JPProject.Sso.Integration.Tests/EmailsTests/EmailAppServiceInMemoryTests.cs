using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.Models;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Fakers.Test.Email;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Infra.Data.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.EmailsTests
{
    public class EmailAppServiceInMemoryTests : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationSsoContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private readonly IEmailAppService _emailAppService;
        public WarmupInMemory InMemoryData { get; }

        public EmailAppServiceInMemoryTests(WarmupInMemory inMemory, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            InMemoryData = inMemory;
            _emailAppService = InMemoryData.Services.GetRequiredService<IEmailAppService>();
            _database = InMemoryData.Services.GetRequiredService<ApplicationSsoContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }

        [Fact]
        public async Task ShouldSaveEmail()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();
        }


        [Fact]
        public async Task ShouldSaveEmailWithManyBccs()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var emailBcc = _faker.Person.Email;
            command.Bcc = $"{emailBcc};{_faker.Internet.Email()};{_faker.Internet.Email()};{_faker.Internet.ExampleEmail()}";

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            var email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);

            email.Bcc.Recipients.Should().Contain(s => s.Equals(emailBcc));
        }

        [Fact]
        public async Task ShouldSaveEmailWithNullBcc()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();

            command.Bcc = null;

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            var email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();
        }


        [Fact]
        public async Task ShouldUpdateEmailWithNullBcc()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();

            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            var email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();

            command.Bcc = new BlindCarbonCopy();
            command.Content = _faker.Lorem.Paragraphs();

            await _emailAppService.SaveEmail(command);
            email = _database.Emails.FirstOrDefault(f => f.Type == command.Type);
            email.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldFindEmailByType()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            await _emailAppService.SaveEmail(command);

            var email = await _emailAppService.FindByType(command.Type);
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();
            email.Should().NotBeNull();
            email.Sender.Should().NotBeNull();
            email.Sender.Name.Should().NotBeNull();
            email.Sender.Address.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldUpdateEmailWhenTypeAlreadyExists()
        {
            var command = EmailFaker.GenerateEmailViewModel().Generate();
            var result = await _emailAppService.SaveEmail(command);
            result.Should().BeTrue();
            _database.Emails.FirstOrDefault(f => f.Type == command.Type).Should().NotBeNull();

            var newCommand = EmailFaker.GenerateEmailViewModel(command.Type).Generate();
            result = await _emailAppService.SaveEmail(newCommand);
            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Emails.Count(f => f.Type == newCommand.Type).Should().Be(1);
        }

        [Fact]
        public async Task ShouldSaveTemplate()
        {
            var command = EmailFaker.GenerateTemplateViewModel().Generate();
            var result = await _emailAppService.SaveTemplate(command);

            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Templates.FirstOrDefault(f => f.Name == command.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldRemoveTemplate()
        {
            var command = EmailFaker.GenerateTemplateViewModel().Generate();
            var result = await _emailAppService.SaveTemplate(command);

            result.Should().BeTrue();

            result = await _emailAppService.RemoveTemplate(command.Name);

            result.Should().BeTrue(becauseArgs: _notifications.GetNotificationsByKey());
            _database.Templates.FirstOrDefault(f => f.Name == command.Name).Should().BeNull();
        }
    }


    public class EmailServiceInMemoryTests : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly ApplicationSsoContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private IEmailService _emailService;
        public WarmupInMemory InMemoryData { get; }

        public EmailServiceInMemoryTests(WarmupInMemory inMemory, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            InMemoryData = inMemory;
            _emailService = InMemoryData.Services.GetRequiredService<IEmailService>();
            _database = InMemoryData.Services.GetRequiredService<ApplicationSsoContext>();
            _notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }

        [Fact]
        public void ShouldReplaceEmailVariables()
        {
            var email = EmailFaker.GenerateEmail().Generate();
            var message = email.GetMessage(UserFaker.GenerateUser().Generate(), new AccountResult(_faker.Random.Guid().ToString(), _faker.Database.Random.AlphaNumeric(5), _faker.Internet.Url()), UserCommandFaker.GenerateRegisterNewUserCommand().Generate());

            message.Content.Should()
                    .NotContain("{{picture}}").And
                    .NotContain("{{name}}").And
                    .NotContain("{{username}}").And
                    .NotContain("{{code}}").And
                    .NotContain("{{url}}").And
                    .NotContain("{{provider}}").And
                    .NotContain("{{phoneNumber}}").And
                    .NotContain("{{email}}");
        }

    }
}
