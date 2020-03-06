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
    public class EmailServiceInMemoryTests : IClassFixture<WarmupUnifiedContext>
    {
        private readonly ITestOutputHelper _output;
        private readonly UnifiedContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private IEmailService _emailService;
        public WarmupUnifiedContext UnifiedContextData { get; }

        public EmailServiceInMemoryTests(WarmupUnifiedContext unifiedContext, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            UnifiedContextData = unifiedContext;
            _emailService = UnifiedContextData.Services.GetRequiredService<IEmailService>();
            _database = UnifiedContextData.Services.GetRequiredService<UnifiedContext>();
            _notifications = (DomainNotificationHandler)UnifiedContextData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }

        [Fact]
        public void Should_Replace_Email_Variables()
        {
            var email = EmailFaker.GenerateEmail().Generate();
            var claims = ClaimFaker.GenerateClaim().Generate(10);
            var message = email.GetMessage(UserFaker.GenerateUser().Generate(), new AccountResult(_faker.Random.Guid().ToString(), _faker.Database.Random.AlphaNumeric(5), _faker.Internet.Url()), UserCommandFaker.GenerateRegisterNewUserCommand().Generate(), claims);

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