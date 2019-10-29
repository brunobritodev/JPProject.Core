using AutoMapper;
using JPProject.EntityFrameworkCore.Configuration;
using JPProject.Sso.Application.AutoMapper;
using JPProject.Sso.Application.Configuration.DependencyInjection;
using JPProject.Sso.Infra.Data.Context;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System;
using System.IO;
using JPProject.Sso.EntityFrameworkCore.SqlServer.Configuration;

namespace JPProject.Sso.Integration.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">Identity user primary key</typeparam>
    public class IdentityWarmup<TKey>
        where TKey : IEquatable<TKey>
    {
        public IdentityWarmup()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("app.json", optional: true, reloadOnChange: true);

            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IConfiguration>(s => builder.Build());

            void DatabaseOptions(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection
                .ConfigureUserIdentity<AspNetUserTest>()
                .WithSqlServer(DatabaseOptions)

                .ConfigureIdentityServer()
                .WithSqlServer(DatabaseOptions)

                .AddEventStoreContext(DatabaseOptions);


            var mappings = SsoMapperConfig.RegisterMappings();
            var automapperConfig = new MapperConfiguration(mappings);
            serviceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            serviceCollection.AddMediatR(typeof(IdentityWarmup<TKey>));
            serviceCollection.TryAddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }
        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<ApplicationIdentityContext>();
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
