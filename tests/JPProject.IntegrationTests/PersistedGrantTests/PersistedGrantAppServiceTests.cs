using FluentAssertions;
using IdentityServer4.EntityFramework.Entities;
using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Application.ViewModels;
using JPProject.Admin.Infra.Data.Context;
using JPProject.Domain.Core.Notifications;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Admin.IntegrationTests.PersistedGrantTests
{
    public class PersistedGrantAppServiceTests : IClassFixture<WarmupInMemory>
    {
        private readonly JpProjectAdminUiContext _database;
        private readonly IPersistedGrantAppService _persistedGrant;
        public WarmupInMemory InMemoryData { get; }

        public PersistedGrantAppServiceTests(WarmupInMemory inMemoryData)
        {
            InMemoryData = inMemoryData;
            _persistedGrant = InMemoryData.Services.GetRequiredService<IPersistedGrantAppService>();
            _database = InMemoryData.Services.GetRequiredService<JpProjectAdminUiContext>();
            var notifications = (DomainNotificationHandler)InMemoryData.Services.GetRequiredService<INotificationHandler<DomainNotification>>();
            notifications.Clear();
        }

        [Fact]
        public async Task Should_Remove_Grant()
        {
            _database.PersistedGrants.Add(new PersistedGrant()
            {
                ClientId = "clientId",
                CreationTime = DateTime.Now,
                Key = "teste"
            });
            await _database.SaveChangesAsync();

            InMemoryData.DetachAll();

            var command = new RemovePersistedGrantViewModel("teste");


            await _persistedGrant.Remove(command);

            _database.PersistedGrants.Count().Should().Be(0);
        }

    }
}
