using AutoMapper;
using AutoMapper.Configuration;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.Configuration;
using JPProject.Sso.Infra.Data.Configuration;
using JPProject.Sso.Infra.Identity.Models.Identity;
using JPProject.Sso.Infra.Identity.Services;
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
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace JPProject.Sso.Integration.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class WarmupUnifiedContext : IWarmupTest
    {
        public WarmupUnifiedContext()
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

            serviceCollection.AddDbContext<UnifiedContext>(DatabaseOptions);


            serviceCollection
                .AddIdentity<UserIdentity, IdentityRole>(AccountOptions.NistAccountOptions)
                .AddEntityFrameworkStores<UnifiedContext>()
                .AddDefaultTokenProviders();

            serviceCollection
                .ConfigureSso<AspNetUserTest, UserService, RoleService>()
                .AddSsoContext<UnifiedContext>()
                .AddDefaultAspNetIdentityServices();

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


            var configurationExpression = new MapperConfigurationExpression();
            SsoMapperConfig.RegisterMappings().ForEach(p => configurationExpression.AddProfile(p));
            var automapperConfig = new MapperConfiguration(configurationExpression);

            serviceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            serviceCollection.AddMediatR(typeof(WarmupUnifiedContext));
            serviceCollection.TryAddSingleton(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<UnifiedContext>();
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
