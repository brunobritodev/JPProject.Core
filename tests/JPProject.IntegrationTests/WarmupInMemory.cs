using AutoMapper;
using JPProject.Admin.Application.AutoMapper;
using JPProject.Admin.Infra.Data.Context;
using JPProject.EntityFrameworkCore.Context;
using JPProject.Fakers.Test;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace JPProject.IntegrationTests
{

    public class WarmupInMemory
    {
        public WarmupInMemory()
        {
            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();


            var mappings = AdminUiMapperConfiguration.RegisterMappings();
            var automapperConfig = new MapperConfiguration(mappings);

            serviceCollection.ConfigureJpAdmin<AspNetUserTest>().WithSqlServer(opt => opt.UseInMemoryDatabase("JpTests").EnableSensitiveDataLogging());
            serviceCollection.TryAddSingleton(automapperConfig.CreateMapper());
            serviceCollection.AddMediatR(typeof(WarmupInMemory));
            serviceCollection.TryAddSingleton<IHttpContextAccessor>(mockHttpContextAccessor.Object);

            Services = serviceCollection.BuildServiceProvider();
        }

        public ServiceProvider Services { get; set; }

        public void DetachAll()
        {

            var database = Services.GetService<JpProjectContext>();
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
