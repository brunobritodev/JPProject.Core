using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Domain.Interfaces;
using JPProject.Sso.Domain.ViewModels.User;
using JPProject.Sso.Fakers.Test.Email;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.EmailsTests
{
    public class EmailServiceInMemoryTests : IClassFixture<WarmupInMemory>
    {
        private readonly ITestOutputHelper _output;
        private readonly SsoContext _database;
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
            _database = InMemoryData.Services.GetRequiredService<SsoContext>();
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