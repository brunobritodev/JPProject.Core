using AutoMapper;
using AutoMapper.Configuration;
using JPProject.EntityFrameworkCore.Context;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.EntityFrameworkCore.SqlServer.Configuration;
using JPProject.Sso.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class WarmupInMemory
    {
        public WarmupInMemory()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<WarmupInMemory>();

            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IConfiguration>(s => builder.Build());

            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection
                .ConfigureUserIdentity<AspNetUserTest>()
                .WithSqlServer(DatabaseOptions)

                .ConfigureIdentityServer()
                .WithSqlServer(DatabaseOptions);

            serviceCollection.AddDbContext<EventStoreContext>(DatabaseOptions);

            var configurationExpression = new MapperConfigurationExpression();
            SsoMapperConfig.RegisterMappings().ForEach(p => configurationExpression.AddProfile(p));
            var automapperConfig = new MapperConfiguration(configurationExpression);

            serviceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            serviceCollection.AddMediatR(typeof(WarmupInMemory));
            serviceCollection.TryAddSingleton(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<ApplicationSsoContext>();
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
