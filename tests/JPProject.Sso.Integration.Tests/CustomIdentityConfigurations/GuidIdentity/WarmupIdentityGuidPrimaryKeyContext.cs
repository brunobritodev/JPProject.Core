using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using JPProject.Sso.Infra.Identity.Configuration;
using JPProject.Sso.Infra.Identity.Models.Identity;
using JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.StringIdentity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.IO;

namespace JPProject.Sso.Integration.Tests.CustomIdentityConfigurations.GuidIdentity
{
    public class WarmupIdentityGuidPrimaryKeyContext : IWarmupTest
    {
        public WarmupIdentityGuidPrimaryKeyContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<WarmupUnifiedContext>();

            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IConfiguration>(s => builder.Build());

            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection.AddDbContext<CustomDatabasePrimaryKey<CustomGuidIdentity, CustomRoleGuidIdentity, Guid>>(DatabaseOptions);


            serviceCollection
                .AddIdentity<CustomGuidIdentity, CustomRoleGuidIdentity>(AccountOptions.NistAccountOptions)
                .AddEntityFrameworkStores<CustomDatabasePrimaryKey<CustomGuidIdentity, CustomRoleGuidIdentity, Guid>>()
                .AddDefaultTokenProviders();

            serviceCollection
                .ConfigureSso<AspNetUserTest>()
                .AddSsoContext<CustomDatabasePrimaryKey<CustomGuidIdentity, CustomRoleGuidIdentity, Guid>>()
                .ConfigureIdentity<CustomGuidIdentity, CustomRoleGuidIdentity, Guid, CustomGuidFactory, CustomGuidFactory>();

            serviceCollection
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddAspNetIdentity<CustomGuidIdentity>()
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

            serviceCollection.AddMediatR(typeof(WarmupUnifiedContext));
            serviceCollection.TryAddSingleton(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<CustomDatabasePrimaryKey<CustomGuidIdentity, CustomRoleGuidIdentity, Guid>>();
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