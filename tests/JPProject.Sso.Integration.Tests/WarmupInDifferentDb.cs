using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using JPProject.Sso.Integration.Tests.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.IO;
using JPProject.Sso.AspNetIdentity.Configuration;
using JPProject.Sso.AspNetIdentity.Models.Identity;
using JPProject.Sso.AspNetIdentity.Services;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace JPProject.Sso.Integration.Tests
{
    public class WarmupInDifferentDb : IWarmupTest
    {
        public WarmupInDifferentDb()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<WarmupInDifferentDb>();

            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IConfiguration>(s => builder.Build());

            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection.AddDbContext<SsoContext>(DatabaseOptions);
            serviceCollection.AddDbContext<EventStoreContext>(DatabaseOptions);


            serviceCollection
                .AddIdentity<UserIdentity, RoleIdentity>(AccountOptions.NistAccountOptions)
                .AddEntityFrameworkStores<SsoContext>()
                .AddDefaultTokenProviders();

            serviceCollection
                .ConfigureSso<AspNetUserTest>()
                .AddSsoContext<SsoContext, EventStoreContext>()
                .ConfigureIdentity<UserIdentity, RoleIdentity, string, IdentityFactory, IdentityFactory>();

            serviceCollection
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<UserIdentity>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = DatabaseOptions;
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = DatabaseOptions;

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 15; // frequency in seconds to cleanup stale grants. 15 is useful during debugging
                });


            serviceCollection.AddMediatR(typeof(WarmupInDifferentDb));
            serviceCollection.TryAddSingleton(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<SsoContext>();
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