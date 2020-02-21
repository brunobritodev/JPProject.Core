using JPProject.Admin.EntityFramework.Repository.Context;
using JPProject.Admin.Fakers.Test;
using JPProject.Admin.IntegrationTests.Contexts;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace JPProject.Admin.IntegrationTests
{

    public class WarmupInMemory
    {
        public WarmupInMemory()
        {
            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();

            void Options(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection.AddDbContext<EventStoreContext>(Options);
            serviceCollection
                .ConfigureJpAdminServices<AspNetUserTest>()
                .AddEventStore<EventStoreContext>()
                .AddJpAdminContext(Options);
            serviceCollection.AddMediatR(typeof(WarmupInMemory));
            Services = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<JpProjectAdminUiContext>();
            foreach (var dbEntityEntry in database.ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    dbEntityEntry.State = EntityState.Detached;
                }
            }

        }
    }
}
