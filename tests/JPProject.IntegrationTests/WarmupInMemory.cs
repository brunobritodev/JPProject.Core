using AutoMapper;
using AutoMapper.Configuration;
using JPProject.Admin.Application.AutoMapper;
using JPProject.Admin.Fakers.Test;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Configuration;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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


            var configurationExpression = new MapperConfigurationExpression();
            AdminUiMapperConfiguration.RegisterMappings().ForEach(p => configurationExpression.AddProfile(p));
            var automapperConfig = new MapperConfiguration(configurationExpression);


            void Options(DbContextOptionsBuilder opt) => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging();

            serviceCollection
                .ConfigureJpAdmin<AspNetUserTest>()
                .WithSqlServer(Options);

            serviceCollection.AddEventStoreContext(Options);
            serviceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            serviceCollection.AddMediatR(typeof(WarmupInMemory));
            serviceCollection.TryAddSingleton(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<JPProjectAdminUIContext>();
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
