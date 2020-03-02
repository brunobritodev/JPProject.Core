using Bogus;
using FluentAssertions;
using JPProject.Domain.Core.Notifications;
using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels.EventsViewModel;
using JPProject.Sso.Fakers.Test.Users;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace JPProject.Sso.Integration.Tests.EventTests
{
    public class EventAppServiceTest : IClassFixture<WarmupUnifiedContext>
    {
        private readonly ITestOutputHelper _output;
        private readonly UnifiedContext _database;
        private readonly Faker _faker;
        private readonly DomainNotificationHandler _notifications;
        private readonly IEventStoreAppService _eventStoreAppService;
        private readonly IUserAppService _userAppService;
        public WarmupUnifiedContext UnifiedContextData { get; }

        public EventAppServiceTest(WarmupUnifiedContext unifiedContext, ITestOutputHelper output)
        {
            _output = output;
            _faker = new Faker();
            UnifiedContextData = unifiedContext;
            _eventStoreAppService = UnifiedContextData.Services.GetRequiredService<IEventStoreAppService>();
            _userAppService = UnifiedContextData.Services.GetRequiredService<IUserAppService>();
            _database = UnifiedContextData.Services.GetRequiredService<UnifiedContext>();
            _notifications = (DomainNotificationHandler)UnifiedContextData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();

            _notifications.Clear();
        }


        [Fact]
        public async Task Should_Get_Events_And_Have_Details_As_Json()
        {
            var command = UserViewModelFaker.GenerateUserViewModel().Generate();
            var result = await _userAppService.Register(command);
            result.Should().BeTrue();
            _database.Users.FirstOrDefault(f => f.UserName == command.Username).Should().NotBeNull();

            var events = _eventStoreAppService.GetEvents(new SearchEventByAggregate());

            events.Collection.Should().HaveCountGreaterOrEqualTo(1);
            foreach (var eventHistoryData in events.Collection)
            {
                eventHistoryData.Details.Should().Contain("{");
            }
        }
    }
}
